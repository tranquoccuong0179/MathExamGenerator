using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamExchange
{
    public class UpdateExamExchangeStatusRequest
    {
        public Guid ExamExchangeId { get; set; }
        public string NewStatus { get; set; } = null!;
    }
}
