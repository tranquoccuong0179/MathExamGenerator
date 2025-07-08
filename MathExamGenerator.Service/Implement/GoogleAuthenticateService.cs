using System.Security.Claims;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Response.GoogleAuthentication;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement;

public class GoogleAuthenticateService : BaseService<GoogleAuthenticateService>, IGoogleAuthenticationService
{
    public GoogleAuthenticateService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<GoogleAuthenticateService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
    }

    public async Task<GoogleAuthResponse> GoogleAuthenticate(HttpContext context)
    {
        var authenticateResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (authenticateResult.Principal == null) return null;
        var name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);
        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        var avatar = authenticateResult.Principal.FindFirstValue("picture");
        if (email == null) return null;
        var accessToken = authenticateResult.Properties.GetTokenValue("access_token");
         
        return new GoogleAuthResponse
        {
            FullName = name,
            Email = email,
            Token = accessToken,
            Avatar = avatar
        };
    }
}