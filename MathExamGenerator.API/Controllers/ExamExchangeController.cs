using MathExamGenerator.API.constant;
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

        // ─────────────── 1. CREATE ───────────────
        [HttpPost(ApiEndPointConstant.ExamEchange.CreateExamExchange)]
        [ProducesResponseType(typeof(BaseResponse<ExamExchangeResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] ExamExchangeRequest request)
        {
            var response = await _service.CreateAsync(request);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
