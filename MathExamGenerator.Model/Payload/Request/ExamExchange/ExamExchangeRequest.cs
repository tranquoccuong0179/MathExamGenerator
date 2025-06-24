using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Request.Question;

namespace MathExamGenerator.Model.Payload.Request.ExamExchange
{
    public class ExamExchangeRequest
    {
        public string CategoryName { get; set; } = default!;
        public string CategoryGrade { get; set; } = default!;
        public List<QuestionRequest> Questions { get; set; } = new();
    }
}
