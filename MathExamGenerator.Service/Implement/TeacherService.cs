using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Request.Teacher;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Teacher;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MathExamGenerator.Service.Implement
{
    public class TeacherService : BaseService<TeacherService>, ITeacherService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IEmailSender _emailSender;
        public TeacherService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, 
                              ILogger<TeacherService> logger, IMapper mapper, 
                              IHttpContextAccessor httpContextAccessor,
                              IConnectionMultiplexer redis,
                              IEmailSender emailSender) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _redis = redis;
            _emailSender = emailSender;
        }

        public async Task<BaseResponse<RegisterTeacherResponse>> RegisterTeacher(RegisterTeacherRequest request)
        {
            var accountList = await _unitOfWork.GetRepository<Account>().GetListAsync();
            if (accountList.Any(a => a.UserName.Equals(request.UserName)))
            {
                throw new BadHttpRequestException("Tên đăng nhập đã tồn tại");
            }

            if (accountList.Any(a => a.Email.Equals(request.Email)))
            {
                throw new BadHttpRequestException("Email đã tồn tại");
            }

            if (accountList.Any(a => a.Phone.Equals(request.Phone)))
            {
                throw new BadHttpRequestException("Số điện thoại đã tồn tại");
            }

            var redisDb = _redis.GetDatabase();
            if (redisDb == null) throw new RedisServerException("Không thể kết nối tới Redis");

            var key = "emailOtp:" + request.Email;
            var storedOtp = await redisDb.StringGetAsync(key);

            if (string.IsNullOrEmpty(storedOtp))
                throw new NotFoundException("Không tìm thấy mã OTP");
            if (!storedOtp.Equals(request.Otp))
                throw new BadHttpRequestException("Mã OTP không chính xác");

            var account = _mapper.Map<Account>(request);

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);

            var location = await _unitOfWork.GetRepository<Location>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(request.LocationId) && l.IsActive == true);

            if (location == null)
            {
                throw new NotFoundException("Không tìm thấy vị trí bạn chọn");
            }

            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                Description = request.Description,
                SchoolName = request.SchoolName,
                LocationId = request.LocationId,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Teacher>().InsertAsync(teacher);

            var wallet = new Wallet()
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                Point = 0,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Wallet>().InsertAsync(wallet);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình đăng ký tài khoản");
            }

            var response = new RegisterTeacherResponse
            {
                Id = account.Id,
                UserName = account.UserName,
                FullName = account.FullName,
                Email = account.Email,
                Phone = account.Phone,
                DateOfBirth = account.DateOfBirth,
                Gender = account.Gender,
                AvatarUrl = account.AvatarUrl,
                Description = teacher.Description,
                SchoolName = teacher.SchoolName,
                Location = location.Name,
            };

            return new BaseResponse<RegisterTeacherResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo tài khoản thành công",
                Data = response
            };
        }
    }
}
