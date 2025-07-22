using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Request.Authentication;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Authentication;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class AuthenticateService : BaseService<AuthenticateService>, IAuthenticateService
    {
        public AuthenticateService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<AuthenticateService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
        {
            Expression<Func<Account, bool>> searchFilter = p =>
                  (p.UserName.Equals(request.UsernameOrEmailOrPhone)
                  || p.Email.Equals(request.UsernameOrEmailOrPhone)
                  || p.Phone.Equals(request.UsernameOrEmailOrPhone)) &&
                  p.Password.Equals(PasswordUtil.HashPassword(request.Password)) &&
                  (p.Role == RoleEnum.ADMIN.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.STAFF.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.MANAGER.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.USER.GetDescriptionFromEnum()) &&
                  p.IsActive == true &&
                  p.DeleteAt == null;
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: searchFilter);

            if (account == null)
            {
                throw new BadHttpRequestException("Tài khoản hoặc mật khẩu không đúng");
            }

            if (account.Role.Equals(RoleEnum.USER.GetDescriptionFromEnum()))
            {
                var today = DateOnly.FromDateTime(TimeUtil.GetCurrentSEATime());

                if (account.DailyLoginRewardedAt == null || account.DailyLoginRewardedAt.Value < today)
                {
                    account.DailyLoginRewardedAt = today;

                    _unitOfWork.GetRepository<Account>().UpdateAsync(account);

                    if (account.Role.Equals(RoleEnum.USER.GetDescriptionFromEnum()))
                    {
                        var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                            predicate: w => w.AccountId.Equals(account.Id) && w.IsActive == true);

                        wallet.Point += 1;
                        
                        _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
                    }
                    
                    await _unitOfWork.CommitAsync();
                }
            }

            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role);
            Tuple<string, Guid> guildClaim = new Tuple<string, Guid>("accountId", account.Id);
            var token = JwtUtil.GenerateJwtToken(account, guildClaim);

                var response = new AuthenticateResponse
            {
                AccessToken = token,
                AccountId = account.Id,
                AvatarUrl = account.AvatarUrl,
                Email = account.Email,
                FullName = account.FullName,
                Phone = account.Phone,
                Username = account.UserName,
                Role = account.Role
            };

            return new BaseResponse<AuthenticateResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Đăng nhập thành công",
                Data = response
            };
        }
    }
}
