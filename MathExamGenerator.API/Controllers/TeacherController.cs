using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Request.Teacher;
using MathExamGenerator.Model.Payload.Response.Teacher;
using MathExamGenerator.Model.Paginate;
using Microsoft.AspNetCore.Authorization;

namespace MathExamGenerator.API.Controllers
{
    public class TeacherController : BaseController<TeacherController>
    {
        private readonly ITeacherService _teacherService;
        public TeacherController(ILogger<TeacherController> logger, ITeacherService teacherService) : base(logger)
        {
            _teacherService = teacherService;
        }

        [HttpPost(ApiEndPointConstant.Teacher.RegisterTeacher)]
        [ProducesResponseType(typeof(BaseResponse<RegisterTeacherResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<RegisterTeacherResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<RegisterTeacherResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RegisterTeacher([FromForm] RegisterTeacherRequest request)
        {
            var response = await _teacherService.RegisterTeacher(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpGet(ApiEndPointConstant.Teacher.GetAllTeacher)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetTeacherResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllTeacher([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _teacherService.GetAllTeacher(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }
        
        [HttpGet(ApiEndPointConstant.Teacher.GetTeacher)]
        [ProducesResponseType(typeof(BaseResponse<GetTeacherResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetTeacherResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTeacher([FromRoute] Guid id)
        {
            var response = await _teacherService.GetTeacher(id);
            return StatusCode(int.Parse(response.Status), response);
        }
        
        [HttpPut(ApiEndPointConstant.Teacher.UpdateTeacher)]
        [ProducesResponseType(typeof(BaseResponse<GetTeacherResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetTeacherResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateTeacher([FromBody] UpdateTeacherRequest request)
        {
            var response = await _teacherService.UpdateTeacher(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Teacher.DeleteTeacher)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteTeacher([FromRoute] Guid id)
        {
            var response = await _teacherService.DeleteTeacher(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
