using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.ExamExchange
{
    public class ExamExchangeResponse
    {
        public Guid TeacherId { get; set; }            
        public string Status { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public string CategoryGrade { get; set; } = default!;
        public List<QuestionResponse> Questions { get; set; } = new();
    }
}
