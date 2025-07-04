﻿using System;
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

                var user = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                    predicate: u => u.AccountId.Equals(account.Id) && u.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin người dùng");

                if (account.DailyLoginRewardedAt == null || account.DailyLoginRewardedAt.Value < today)
                {
                    account.DailyLoginRewardedAt = today;

                    _unitOfWork.GetRepository<Account>().UpdateAsync(account);

                    user.Point += 10;

                    _unitOfWork.GetRepository<UserInfo>().UpdateAsync(user);
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
