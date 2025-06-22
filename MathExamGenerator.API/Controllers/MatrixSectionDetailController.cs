using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.MatrixSectionDetail;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Request.MatrixSectionDetail;

namespace MathExamGenerator.API.Controllers
{
    public class MatrixSectionDetailController : BaseController<MatrixSectionDetailController>
    {
        private readonly IMatrixSectionDetailService _matrixSectionDetailService;

        public MatrixSectionDetailController(ILogger<MatrixSectionDetailController> logger, IMatrixSectionDetailService matrixSectionDetailService) : base(logger)
        {
            _matrixSectionDetailService = matrixSectionDetailService;
        }

        [HttpGet(ApiEndPointConstant.MatrixSectionDetail.GetAll)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<MatrixSectionDetailResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<MatrixSectionDetailResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;

            var result = await _matrixSectionDetailService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpGet(ApiEndPointConstant.MatrixSectionDetail.GetById)]
        [ProducesResponseType(typeof(BaseResponse<MatrixSectionDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<MatrixSectionDetailResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _matrixSectionDetailService.GetById(id);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpPut(ApiEndPointConstant.MatrixSectionDetail.Update)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMatrixSectionDetailRequest request)
        {
            var result = await _matrixSectionDetailService.UpdateDetail(id, request);
            return StatusCode(int.Parse(result.Status), result);
        }

        [HttpDelete(ApiEndPointConstant.MatrixSectionDetail.Delete)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _matrixSectionDetailService.DeleteDetail(id);
            return StatusCode(int.Parse(result.Status), result);
        }
    }
}
