
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.User;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.User;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }

        [HttpGet(ApiEndPointConstant.User.GetAllUsers)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetUserResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllUsers([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _userService.GetAllUsers(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.User.GetUser)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var response = await _userService.GetUser(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.User.GetUserProfile)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetUserProfile()
        {
            var response = await _userService.GetUserProfile();
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.User.UpdateUser)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetUserResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var response = await _userService.UpdateUser(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.User.DeleteUser)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var response = await _userService.DeleteUser(id);
            return StatusCode(int.Parse(response.Status), response);
        }
        
        [HttpPost(ApiEndPointConstant.User.ExchangePoint)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> ExchangePoint()
        {
            var response = await _userService.ExchangePoint();
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
