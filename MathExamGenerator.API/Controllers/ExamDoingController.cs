using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.ExamDoing;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Request.ExamDoing;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Enum;

namespace MathExamGenerator.API.Controllers
{
    public class ExamDoingController : BaseController<ExamDoingController>
    {
        private readonly IExamDoingService _examDoingService;

        public ExamDoingController(ILogger<ExamDoingController> logger, IExamDoingService examDoingService)
            : base(logger)
        {
            _examDoingService = examDoingService;
        }

        [HttpGet(ApiEndPointConstant.ExamDoing.GetAll)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<ExamDoingOverviewResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<ExamDoingOverviewResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;

            var result = await _examDoingService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpGet(ApiEndPointConstant.ExamDoing.GetById)]
        [ProducesResponseType(typeof(BaseResponse<GetExamDoingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetExamDoingResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _examDoingService.GetById(id);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPost(ApiEndPointConstant.ExamDoing.Create)]
        [ProducesResponseType(typeof(BaseResponse<CreateExamDoingResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<CreateExamDoingResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateExamDoingResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CreateExamDoingResponse>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CreateExamDoingRequest request)
        {
            var result = await _examDoingService.Create(request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPut(ApiEndPointConstant.ExamDoing.Update)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExamDoingRequest request, [FromQuery] ExamDoingEnum? status)
        {
            var result = await _examDoingService.Update(id, request, status);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpDelete(ApiEndPointConstant.ExamDoing.Delete)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _examDoingService.Delete(id);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpGet(ApiEndPointConstant.ExamDoing.GetQuestionHistoriesByTestId)]
        [ProducesResponseType(typeof(BaseResponse<List<GetQuestionHistoryResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<GetQuestionHistoryResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionHistoriesByTestId([FromRoute] Guid id)
        {
            var result = await _examDoingService.GetQuestionHistoriesByTestId(id);
            return StatusCode(int.Parse(result.Status), result);
        }
    }
}
