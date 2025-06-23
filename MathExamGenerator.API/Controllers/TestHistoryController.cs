using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.TestHistory;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Request.TestHistory;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;

namespace MathExamGenerator.API.Controllers
{
    public class TestHistoryController : BaseController<TestHistoryController>
    {
        private readonly ITestHistoryService _testHistoryService;

        public TestHistoryController(ILogger<TestHistoryController> logger, ITestHistoryService testHistoryService)
            : base(logger)
        {
            _testHistoryService = testHistoryService;
        }

        [HttpGet(ApiEndPointConstant.TestHistory.GetAll)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<TestHistoryOverviewResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<TestHistoryOverviewResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;

            var result = await _testHistoryService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpGet(ApiEndPointConstant.TestHistory.GetById)]
        [ProducesResponseType(typeof(BaseResponse<GetTestHistoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetTestHistoryResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _testHistoryService.GetById(id);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPost(ApiEndPointConstant.TestHistory.Create)]
        [ProducesResponseType(typeof(BaseResponse<CreateTestHistoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<CreateTestHistoryResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateTestHistoryResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CreateTestHistoryResponse>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CreateTestHistoryRequest request)
        {
            var result = await _testHistoryService.Create(request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPut(ApiEndPointConstant.TestHistory.Update)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTestHistoryRequest request)
        {
            var result = await _testHistoryService.Update(id, request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpDelete(ApiEndPointConstant.TestHistory.Delete)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _testHistoryService.Delete(id);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpGet(ApiEndPointConstant.TestHistory.GetQuestionHistoriesByTestId)]
        [ProducesResponseType(typeof(BaseResponse<List<GetQuestionHistoryResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<GetQuestionHistoryResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionHistoriesByTestId([FromRoute] Guid id)
        {
            var result = await _testHistoryService.GetQuestionHistoriesByTestId(id);
            return StatusCode(int.Parse(result.Status), result);
        }
    }
}
