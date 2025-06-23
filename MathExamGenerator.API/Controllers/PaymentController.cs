using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.Payment;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Payment;
using MathExamGenerator.Service.Implement;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class PaymentController : BaseController<PaymentController>
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService) : base(logger)
        {
            _paymentService = paymentService;
        }

        [HttpPost(ApiEndPointConstant.Payment.CreatePayment)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            var response = await _paymentService.Create(request);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpPost(ApiEndPointConstant.Payment.HandleWebhook)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> HandleWebhook([FromBody] WebhookNotification notification)
        {
            var response = await _paymentService.HandleWebhookAsync(notification);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
