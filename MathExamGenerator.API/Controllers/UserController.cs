
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.User;

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

        [HttpDelete(ApiEndPointConstant.User.DeleteUser)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var response = await _userService.DeleteUser(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
