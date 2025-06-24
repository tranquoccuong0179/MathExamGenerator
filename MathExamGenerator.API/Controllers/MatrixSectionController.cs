using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response.MatrixSection;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.MatrixSection;
using MathExamGenerator.Model.Payload.Response.MatrixSectionDetail;

namespace MathExamGenerator.API.Controllers
{
    public class MatrixSectionController : BaseController<MatrixSectionController>
    {
        private readonly IMatrixSectionService _matrixSectionService;
        private readonly IMatrixSectionDetailService _detailService;

        public MatrixSectionController(ILogger<MatrixSectionController> logger, IMatrixSectionService matrixSectionService, IMatrixSectionDetailService detailService)
            : base(logger)
        {
            _matrixSectionService = matrixSectionService;
            _detailService = detailService;
        }

        [HttpGet(ApiEndPointConstant.MatrixSection.GetAllMatrixSection)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<MatrixSectionStructureResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<MatrixSectionStructureResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;

            var response = await _matrixSectionService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.MatrixSection.GetMatrixSection)]
        [ProducesResponseType(typeof(BaseResponse<MatrixSectionStructureResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<MatrixSectionStructureResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _matrixSectionService.GetById(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.MatrixSection.DeleteMatrixSection)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _matrixSectionService.DeleteSection(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.MatrixSection.GetAllDetailsBySectionId)]
        [ProducesResponseType(typeof(BaseResponse<List<MatrixSectionDetailResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<MatrixSectionDetailResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllDetailsBySectionId([FromRoute] Guid id)
        {
            var response = await _detailService.GetAllBySectionId(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
