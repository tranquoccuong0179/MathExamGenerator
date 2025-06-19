using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.Exam;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Service.Implement;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class ExamController : BaseController<ExamController>
    {
        private readonly IExamService _examService;

        public ExamController(ILogger<ExamController> logger, IExamService examService) : base(logger)
        {
            _examService = examService;
        }

        [HttpPost(ApiEndPointConstant.Exam.CreateExam)]
        [ProducesResponseType(typeof(BaseResponse<CreateExamResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateExamResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CreateExamResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> RegisterTeacher([FromForm] CreateExamRequest request)
        {
            var response = await _examService.CreateExam(request);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
