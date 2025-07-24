using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Payment
{
    public class PaymentExamRequest
    {
        public Guid ExamDoingId { get; set; }
        public int Amount { get; set; }
    }
}
