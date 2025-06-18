using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Payload.Request.Authentication;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Authentication;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
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
                  p.Role == RoleEnum.TEACHER.GetDescriptionFromEnum() ||
                  p.Role == RoleEnum.USER.GetDescriptionFromEnum()) &&
                  p.IsActive == true;
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: searchFilter);

            if (account == null)
            {
                throw new BadHttpRequestException("Tài khoản hoặc mật khẩu không đúng");
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
