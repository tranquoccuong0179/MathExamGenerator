using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.User;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Authentication;
using MathExamGenerator.Model.Payload.Response.GoogleAuthentication;
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
        public UserService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, 
                           ILogger<UserService> logger, 
                           IMapper mapper, 
                           IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<bool>> DeleteUser(Guid id)
        {

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(id) && a.Role.Equals(RoleEnum.USER.GetDescriptionFromEnum()) && a.IsActive == true);

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
            if (page < 1 || size < 1)
            {
                throw new BadHttpRequestException("Số trang và số lượng trong trang phải lớn hơn hoặc bằng 1");
            }

            var users = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                selector: u => new GetUserResponse
                {
                    AccountId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    DateOfBirth = u.DateOfBirth,
                    AvatarUrl = u.AvatarUrl,
                    Gender = u.Gender,
                    FreeTries = u.FreeTries,
                    Point = u.Wallets.FirstOrDefault().Point,
                    IsPremium = u.IsPremium,
                },
                predicate: u => u.IsActive == true && u.Role .Equals(RoleEnum.USER.GetDescriptionFromEnum()),
                include: u => u.Include(u => u.Wallets),
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
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                selector: u => new GetUserResponse
                {
                    AccountId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    DateOfBirth = u.DateOfBirth,
                    AvatarUrl = u.AvatarUrl,
                    Gender = u.Gender,
                    FreeTries = u.FreeTries,
                    Point = u.Wallets.FirstOrDefault().Point,
                    IsPremium = u.IsPremium
                },
                predicate: u => u.IsActive == true && u.Id.Equals(id) && u.Role .Equals(RoleEnum.USER.GetDescriptionFromEnum()),
                include: u => u.Include(u => u.Wallets));


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

        public async Task<BaseResponse<GetUserResponse>> GetUserProfile()
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true,
                include: u => u.Include(u => u.Wallets))
                              ?? throw new NotFoundException("Không tìm thấy tài khoản");

            return new BaseResponse<GetUserResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin người dùng thành công",
                Data = new GetUserResponse
                {
                    AccountId = accountId,
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    AvatarUrl = account.AvatarUrl,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    FreeTries = account.FreeTries,
                    Point = account.Wallets.FirstOrDefault().Point,
                    IsPremium = account.IsPremium,
                }
            };
        }

        public async Task<BaseResponse<GetUserResponse>> UpdateUser(UpdateUserRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true,
                include: u => u.Include(u => u.Wallets))
                              ?? throw new NotFoundException("Không tìm thấy tài khoản người dùng");

            account.FullName = request.FullName ?? account.FullName;
            account.DateOfBirth = request.DateOfBirth ?? account.DateOfBirth;
            account.Gender = request.Gender?.GetDescriptionFromEnum() ?? account.Gender;
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
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    AvatarUrl = account.AvatarUrl,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    FreeTries = account.FreeTries,
                    Point = account.Wallets.FirstOrDefault().Point,
                    IsPremium = account.IsPremium,
                }
            };
        }

        public async Task<bool> GetAccountByEmail(string email)
        {
            if (email == null) throw new BadHttpRequestException("Email cannot be null");

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: p => p.Email.Equals(email)
            );
            return account != null;
        }

        public async Task<BaseResponse<GetUserResponse>> CreateNewUserAccountByGoogle(GoogleAuthResponse googleAuthResponse)
        {
            var existingUser = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: u => u.Email.Equals(googleAuthResponse.Email) &&
                                                        u.IsActive == true,
                include: u => u.Include(u => u.Wallets));

            if (existingUser != null)
            {
                return new BaseResponse<GetUserResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "User account already exists.",
                    Data = new GetUserResponse()
                    {
                        AccountId = existingUser.Id,
                        Email = existingUser.Email,
                        FullName = existingUser.FullName,
                        Phone = existingUser.Phone,
                        AvatarUrl = existingUser.AvatarUrl,
                        DateOfBirth = existingUser.DateOfBirth,
                        Gender = existingUser.Gender,
                        FreeTries = existingUser.FreeTries,
                        Point = existingUser.Wallets.FirstOrDefault().Point,
                        IsPremium = existingUser.IsPremium
                    }
                };
            }

            var account = new Account()
            {
                Id = Guid.NewGuid(),
                Email = googleAuthResponse.Email,
                FullName = googleAuthResponse.FullName,
                UserName = googleAuthResponse.Email.Split("@")[0],
                Role = RoleEnum.USER.GetDescriptionFromEnum(),
                IsActive = true,
                Password = PasswordUtil.HashPassword("12345678"),
                Phone = "0000000000",
                DateOfBirth = DateOnly.FromDateTime(TimeUtil.GetCurrentSEATime()),
                Gender = GenderEnum.Male.GetDescriptionFromEnum(),
                AvatarUrl = googleAuthResponse.Avatar,
                FreeTries = 0,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };
            
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            
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
                throw new Exception("Một lỗi đã xảy ra trong quá trình đăng nhập google");
            }

            return new BaseResponse<GetUserResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo tài khoản thành công",
                Data = new GetUserResponse()
                {
                    AccountId = account.Id,
                    Email = account.Email,
                    FullName = account.FullName,
                    Phone = account.Phone,
                    AvatarUrl = account.AvatarUrl,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    FreeTries = account.FreeTries,
                    Point = wallet.Point,
                    IsPremium = account.IsPremium
                }
            };
        }

        public async Task<BaseResponse<AuthenticateResponse>> CreateTokenByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Username cannot be null or empty", nameof(email));
            }
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: p => p.Email.Equals(email) && p.IsActive == true && p.DeleteAt == null
            );
            if (account == null) throw new NotFoundException("Account not found or has been banned.");
            Tuple<string, Guid> guildClaim = new Tuple<string, Guid>("accountId", account.Id);
            var token = JwtUtil.GenerateJwtToken(account, guildClaim);

            var response = new AuthenticateResponse()
            {
                AccountId = account.Id,
                Email = account.Email,
                Username = account.UserName,
                Phone = account.Phone,
                Role = account.Role,
                FullName = account.FullName,
                AccessToken = token,
                AvatarUrl = account.AvatarUrl,
            };

            return new BaseResponse<AuthenticateResponse>()
            {
                Status = StatusCodes.Status200OK.ToString(), 
                Message = "Login successful",
                Data = response
            };
        }

        public async Task<BaseResponse<bool>> ExchangePoint()
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true,
                include: u => u.Include(u => u.Wallets)) 
                              ?? throw new NotFoundException("Không tìm thấy tài khoản");
            
            if (account.Wallets.FirstOrDefault().Point < 50)
            {
                throw new BadHttpRequestException("Số điểm của bạn không đủ để đổi lượt kiểm tra miễn phí, tối thiểu phải 50 điểm");
            }

            account.FreeTries += 1;
            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            
            account.Wallets.FirstOrDefault().Point -= 50;
            
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            
            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình thực hiện đổi điểm");
            }

            return new BaseResponse<bool>()
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Đổi điểm sang lượt kiểm tra miễn phí thành công",
                Data = isSuccess
            };
        }
    }
}
