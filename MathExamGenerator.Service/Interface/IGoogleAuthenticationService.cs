using MathExamGenerator.Model.Payload.Response.GoogleAuthentication;
using Microsoft.AspNetCore.Http;

namespace MathExamGenerator.Service.Interface;

public interface IGoogleAuthenticationService
{
    Task<GoogleAuthResponse> GoogleAuthenticate(HttpContext context);
}