using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.User;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.User;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class UserService : BaseService<UserService>, IUserService
    {
        public UserService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<bool>> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: u => u.Id.Equals(id) && u.IsActive == true);

            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy thông tin người dùng");
            }

            user.IsActive = false;
            user.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<UserInfo>().UpdateAsync(user);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(user.AccountId) && a.IsActive == true);

            if (account == null)
            {
                throw new NotFoundException("Không tìm thấy tài khoản người dùng");
            }

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
                Message = "Xóa tài khoản thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetUserResponse>>> GetAllUsers(int page, int size)
        {
            var users = await _unitOfWork.GetRepository<UserInfo>().GetPagingListAsync(
                selector: u => new GetUserResponse
                {
                    AccountId = u.AccountId,
                    UserId = u.Id,
                    FullName = u.Account.FullName,
                    Email = u.Account.Email,
                    Phone = u.Account.Phone,
                    DateOfBirth = u.Account.DateOfBirth,
                    Gender = u.Account.Gender,
                    QuizFree = u.Account.QuizFree,
                    Point = u.Point
                },
                predicate: u => u.IsActive == true,
                include: u => u.Include(u => u.Account),
                orderBy: u => u.OrderByDescending(u => u.CreateAt),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetUserResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách thông tin người dùng thành công",
                Data = users
            };
        }

        public async Task<BaseResponse<GetUserResponse>> GetUser(Guid id)
        {
            var user = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                selector: u => new GetUserResponse
                {
                    AccountId = u.AccountId,
                    UserId = u.Id,
                    FullName = u.Account.FullName,
                    Email = u.Account.Email,
                    Phone = u.Account.Phone,
                    DateOfBirth = u.Account.DateOfBirth,
                    Gender = u.Account.Gender,
                    QuizFree = u.Account.QuizFree,
                    Point = u.Point
                },
                predicate: u => u.IsActive == true && u.Id.Equals(id),
                include: u => u.Include(u => u.Account));


            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy thông tin người dùng");
            }

            return new BaseResponse<GetUserResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin người dùng thành công",
                Data = user
            };
        }

        public async Task<BaseResponse<GetUserResponse>> UpdateUser(UpdateUserRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản người dùng");

            var user = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(accountId) && u.IsActive == true) ?? throw new NotFoundException("Không tìm thấy người dùng");

            account.FullName = request.FullName ?? account.FullName;
            account.DateOfBirth = request.DateOfBirth ?? account.DateOfBirth;
            account.Gender = request.Gender.GetDescriptionFromEnum() ?? account.Gender;
            account.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật tài khoản");
            }

            return new BaseResponse<GetUserResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật tài khoản thành công",
                Data = new GetUserResponse
                {
                    AccountId = account.Id,
                    UserId = user.Id,
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    QuizFree = account.QuizFree,
                    Point = user.Point
                }
            };
        }
    }
}
