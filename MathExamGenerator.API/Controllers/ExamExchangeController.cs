using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamExchange;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamExchange;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MathExamGenerator.API.Controllers
{
    public class ExamExchangeController : BaseController<ExamExchangeController>
    {
        private readonly IExamExchangeService _service;

        public ExamExchangeController(
            ILogger<ExamExchangeController> logger,
            IExamExchangeService service
            )
            : base(logger)
        {
            _service = service;
        }

        [HttpPost(ApiEndPointConstant.ExamEchange.CreateExamExchange)]
        [ProducesResponseType(typeof(BaseResponse<ExamExchangeResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] ExamExchangeRequest request)
        {
            var response = await _service.Create(request);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpGet(ApiEndPointConstant.ExamEchange.GetExamExchangeByTeacher)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<ExamExchangeResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetExamExchangeByTeacher([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _service.GetByTeacher(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpGet(ApiEndPointConstant.ExamEchange.GetExamExchangeById)]
        [ProducesResponseType(typeof(BaseResponse<ExamExchangeResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetExamExchangeById([FromRoute] Guid id)
        {
            var response = await _service.GetExamChange(id);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpPut(ApiEndPointConstant.ExamEchange.UpdateExamExchange)]
        [ProducesResponseType(typeof(BaseResponse<ExamExchangeResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateExamExchange([FromRoute] Guid id, [FromBody] UpdateExamEchangeRequest request)
        {
            var response = await _service.Update(id, request);
            return StatusCode(int.Parse(response.Status), response);

        }
        [HttpDelete(ApiEndPointConstant.ExamEchange.DeleteExamExchange)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteExamExchange([FromRoute] Guid id)
        {
            var response = await _service.Delete(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.ExamEchange.GetAllTeacher)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetExamExchangeTeacherResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllTeacher([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _service.GetAllTeacher(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpPost(ApiEndPointConstant.ExamEchange.ApproveExamExchange)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> ApproveExamExchange([FromBody] UpdateExamExchangeStatusRequest request)
        {
            var response = await _service.ApproveExamExchange(request);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
