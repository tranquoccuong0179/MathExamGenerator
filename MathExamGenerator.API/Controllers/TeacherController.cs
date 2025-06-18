
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Request.Teacher;
using MathExamGenerator.Model.Payload.Response.Teacher;

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
    }
}
