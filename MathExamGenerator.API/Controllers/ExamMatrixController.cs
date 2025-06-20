using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.ExamMatrix;
using MathExamGenerator.Model.Payload.Response.ExamMatrix;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

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
    }
}
