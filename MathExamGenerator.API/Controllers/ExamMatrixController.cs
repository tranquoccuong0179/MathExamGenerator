using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.ExamMatrix;
using MathExamGenerator.Model.Payload.Response.ExamMatrix;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Paginate;

namespace MathExamGenerator.API.Controllers
{
    public class ExamMatrixController : BaseController<ExamMatrixController>
    {
        private readonly IExamMatrixService _examMatrixService;

        public ExamMatrixController(ILogger<ExamMatrixController> logger, IExamMatrixService examMatrixService)
            : base(logger)
        {
            _examMatrixService = examMatrixService;
        }

        [HttpPost(ApiEndPointConstant.ExamMatrix.CreateExamMatrix)]
        [ProducesResponseType(typeof(BaseResponse<GetExamMatrixResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetExamMatrixResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<GetExamMatrixResponse>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateExamMatrix([FromBody] CreateExamMatrixWithStructureRequest request)
        {
            var response = await _examMatrixService.CreateExamMatrix(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.ExamMatrix.GetAllExamMatrix)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamMatrixResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamMatrixResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllExamMatrix([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;

            var response = await _examMatrixService.GetAllExamMatrix(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.ExamMatrix.DeleteExamMatrix)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteExamMatrix([FromRoute] Guid id)
        {
            var response = await _examMatrixService.DeleteExamMatrix(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.ExamMatrix.GetMatrixStructure)]
        [ProducesResponseType(typeof(BaseResponse<ExamMatrixStructureResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ExamMatrixStructureResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetMatrixStructure([FromRoute] Guid id)
        {
            var response = await _examMatrixService.GetMatrixStructure(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.ExamMatrix.UpdateExamMatrix)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateExamMatrix([FromRoute] Guid id, [FromBody] UpdateExamMatrixRequest request)
        {
            var response = await _examMatrixService.UpdateExamMatrix(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.ExamMatrix.GetExamMatrix)]
        [ProducesResponseType(typeof(BaseResponse<GetExamMatrixResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetExamMatrixResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetExamMatrixById([FromRoute] Guid id)
        {
            var response = await _examMatrixService.GetById(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
