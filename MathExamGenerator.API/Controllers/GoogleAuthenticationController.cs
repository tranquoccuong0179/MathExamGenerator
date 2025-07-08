using MathExamGenerator.API.constant;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers;

public class GoogleAuthenticationController : BaseController<GoogleAuthenticationController>
{
    private readonly IGoogleAuthenticationService _googleAuthenticationService;
    private readonly IUserService _userService;
    public GoogleAuthenticationController(ILogger<GoogleAuthenticationController> logger, IGoogleAuthenticationService googleAuthenticationService, IUserService userService) : base(logger)
    {
        _googleAuthenticationService = googleAuthenticationService;
        _userService = userService;
    }
    
    [HttpGet(ApiEndPointConstant.GoogleAuthentication.GoogleAuthLogin)]
    public IActionResult Login()
    {
        var props = new AuthenticationProperties() { RedirectUri = $"api/v1/google-auth/signin-google" };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet("google-auth/signin-google")]
    public async Task<IActionResult> SignInGoogle()
    {
        var googleAuthResponse = await _googleAuthenticationService.GoogleAuthenticate(HttpContext);
        var checkAccount = await _userService.GetAccountByEmail(googleAuthResponse.Email);
        if (!checkAccount)
        {
            var response = await _userService.CreateNewUserAccountByGoogle(googleAuthResponse);
            if (response == null)
            {
                return Problem("Tài khoản không tồn tại");
            }
        }
        var token = await _userService.CreateTokenByEmail(googleAuthResponse.Email);
        return StatusCode(int.Parse(token.Status), token);
    }
}