
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.Authentication;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Authentication;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IAuthenticateService _authService;
        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticateService authService) : base(logger)
        {
            _authService = authService;
        }

        [HttpPost(ApiEndPointConstant.Authentication.Authenticate)]
        [ProducesResponseType(typeof(BaseResponse<AuthenticateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AuthenticateResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var response = await _authService.Authenticate(request);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
