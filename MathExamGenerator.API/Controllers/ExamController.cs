using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Paginate;
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
        public async Task<IActionResult> CreateExam([FromBody] CreateExamRequest request)
        {
            var response = await _examService.CreateExam(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Exam.DeleteExam)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteExam([FromRoute] Guid id)
        {
            var response = await _examService.DeleteExam(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.Exam.UpdateExam)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateExam([FromRoute] Guid id, [FromBody] UpdateExamRequest request, [FromQuery] ExamEnum? status)
        {
            var response = await _examService.UpdateExam(id, request, status);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Exam.GetAllExam)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllExam([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _examService.GetAllExam(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Exam.GetExamsOfCurrentUser)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamResponse>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetExamsOfCurrentUser([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _examService.GetExamsOfCurrentUser(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Exam.GetExam)]
        [ProducesResponseType(typeof(BaseResponse<GetExamResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetExamResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetExamById([FromRoute] Guid id)
        {
            var response = await _examService.GetById(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Exam.GetAllQuestionByExam)]
        [ProducesResponseType(typeof(BaseResponse<ExamWithQuestionsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ExamWithQuestionsResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionsByExamId([FromRoute] Guid id)
        {
            var response = await _examService.GetAllQuestionByExam(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
