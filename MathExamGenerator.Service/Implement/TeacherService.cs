using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Teacher;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Teacher;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MathExamGenerator.Service.Implement
{
    public class TeacherService : BaseService<TeacherService>, ITeacherService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IUploadService _uploadService;
        public TeacherService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, 
                              ILogger<TeacherService> logger, IMapper mapper, 
                              IHttpContextAccessor httpContextAccessor,
                              IConnectionMultiplexer redis,
                              IUploadService uploadService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _redis = redis;
            _uploadService = uploadService;
        }

        public async Task<BaseResponse<bool>> DeleteTeacher(Guid id)
        {
            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.IsActive == true) ?? throw new NotFoundException("Không tìm thấy giáo viên");

            teacher.IsActive = false;
            teacher.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teacher);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(teacher.AccountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản giáo viên");

            account.IsActive = false;
            account.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình xóa tài khoản");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa giáo viên thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetTeacherResponse>>> GetAllTeacher(int page, int size)
        {
            var teachers = await _unitOfWork.GetRepository<Teacher>().GetPagingListAsync(
                selector: t => new GetTeacherResponse
                {
                    AccountId = t.AccountId.Value,
                    TeacherId = t.Id,
                    FullName = t.Account.FullName,
                    Email = t.Account.Email,
                    Phone = t.Account.Phone,
                    DateOfBirth = t.Account.DateOfBirth,
                    Gender = t.Account.Gender,
                    Description = t.Description,
                    SchoolName = t.SchoolName,
                    LocationName = t.Location.Name
                },
                predicate: t => t.IsActive == true,
                orderBy: t => t.OrderByDescending(t => t.CreateAt),
                include: t => t.Include(t => t.Account).Include(t => t.Location),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetTeacherResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách giáo viên thành công",
                Data = teachers
            };
        }

        public async Task<BaseResponse<GetTeacherResponse>> GetTeacher(Guid id)
        {
            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                selector: t => new GetTeacherResponse
                {
                    AccountId = t.AccountId.Value,
                    TeacherId = t.Id,
                    FullName = t.Account.FullName,
                    Email = t.Account.Email,
                    Phone = t.Account.Phone,
                    DateOfBirth = t.Account.DateOfBirth,
                    Gender = t.Account.Gender,
                    Description = t.Description,
                    SchoolName = t.SchoolName,
                    LocationName = t.Location.Name
                },
                predicate: t => t.Id.Equals(id) && t.IsActive == true,
                orderBy: t => t.OrderByDescending(t => t.CreateAt),
                include: t => t.Include(t => t.Account).Include(t => t.Location));

            if (teacher == null)
            {
                throw new NotFoundException("Không tìm thấy giáo viên");
            }

            return new BaseResponse<GetTeacherResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin giáo viên thành công",
                Data = teacher
            };
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

            account.AvatarUrl = await _uploadService.UploadImage(request.AvatarUrl);

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);

            var location = await _unitOfWork.GetRepository<Location>().SingleOrDefaultAsync(
                predicate: l => l.Id.Equals(request.LocationId) && l.IsActive == true) ?? throw new NotFoundException("Không tìm thấy vị trí bạn chọn");

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

        public async Task<BaseResponse<GetTeacherResponse>> UpdateTeacher(UpdateTeacherRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            account.FullName = request.FullName ?? account.FullName;
            account.DateOfBirth = request.DateOfBirth ?? account.DateOfBirth;
            account.Gender = request.Gender.GetDescriptionFromEnum() ?? account.Gender;
            account.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.IsActive == true) ?? throw new NotFoundException("Không tìm thấy giáo viên");

            if (request.LocationId.HasValue)
            {
                var location = await _unitOfWork.GetRepository<Location>().SingleOrDefaultAsync(
                    predicate: l => l.Id.Equals(request.LocationId) && l.IsActive == true) ?? throw new NotFoundException("Không tìm thấy vị trí");
            }

            teacher.Description = request.Description ?? teacher.Description;
            teacher.SchoolName = request.SchoolName ?? teacher.SchoolName;
            teacher.LocationId = request.LocationId ?? teacher.LocationId;
            teacher.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teacher);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật tài khoản");
            }

            return new BaseResponse<GetTeacherResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật thành công",
                Data = new GetTeacherResponse
                {
                    AccountId = account.Id,
                    TeacherId = teacher.Id,
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    Description = teacher.Description,
                    SchoolName = teacher.SchoolName,
                    LocationName = teacher.Location.Name
                }
            };
        }
    }
}
