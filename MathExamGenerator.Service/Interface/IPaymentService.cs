using MathExamGenerator.Model.Payload.Request.Payment;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Payment;
using MathExamGenerator.Service.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IPaymentService
    {
        Task<BaseResponse<string>> Create(PaymentRequest request);
        Task<BaseResponse<string>> HandleWebhook(WebhookNotification notification);
    }
}
