using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.TestStorage;
using MathExamGenerator.Model.Payload.Response.TestStorage;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class TestStorageController : BaseController<TestStorageController>
    {
        private readonly ITestStorageService _testStorageService;

        public TestStorageController(ILogger<TestStorageController> logger, ITestStorageService testStorageService) : base(logger)
        {
            _testStorageService = testStorageService;
        }

        [HttpGet(ApiEndPointConstant.TestStorage.GetAll)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetTestStorageResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetTestStorageResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;

            var result = await _testStorageService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpGet(ApiEndPointConstant.TestStorage.GetById)]
        [ProducesResponseType(typeof(BaseResponse<GetTestStorageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetTestStorageResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _testStorageService.GetById(id);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPost(ApiEndPointConstant.TestStorage.Create)]
        [ProducesResponseType(typeof(BaseResponse<GetTestStorageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetTestStorageResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<GetTestStorageResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<GetTestStorageResponse>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CreateTestStorageRequest request)
        {
            var result = await _testStorageService.Create(request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPut(ApiEndPointConstant.TestStorage.Update)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTestStorageRequest request)
        {
            var result = await _testStorageService.Update(id, request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpDelete(ApiEndPointConstant.TestStorage.Delete)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _testStorageService.Delete(id);
            return StatusCode(int.Parse(result.Status), result);
        }
    }
}
