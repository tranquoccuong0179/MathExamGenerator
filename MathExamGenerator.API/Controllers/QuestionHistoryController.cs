using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class QuestionHistoryController : BaseController<QuestionHistoryController>
    {
        private readonly IQuestionHistoryService _questionHistoryService;

        public QuestionHistoryController(ILogger<QuestionHistoryController> logger, IQuestionHistoryService questionHistoryService) : base(logger)
        {
            _questionHistoryService = questionHistoryService;
        }

        [HttpGet(ApiEndPointConstant.QuestionHistory.GetAll)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetQuestionHistoryResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetQuestionHistoryResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;

            var result = await _questionHistoryService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpGet(ApiEndPointConstant.QuestionHistory.GetById)]
        [ProducesResponseType(typeof(BaseResponse<GetQuestionHistoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetQuestionHistoryResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _questionHistoryService.GetById(id);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPost(ApiEndPointConstant.QuestionHistory.Create)]
        [ProducesResponseType(typeof(BaseResponse<CreateQuestionHistoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateQuestionHistoryResponse>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CreateQuestionHistoryRequest request)
        {
            var result = await _questionHistoryService.Create(request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPut(ApiEndPointConstant.QuestionHistory.Update)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateQuestionHistoryRequest request)
        {
            request.Id = id;
            var result = await _questionHistoryService.Update(id, request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpDelete(ApiEndPointConstant.QuestionHistory.Delete)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _questionHistoryService.Delete(id);
            return StatusCode(int.Parse(result.Status), result);
        }
    }
}
