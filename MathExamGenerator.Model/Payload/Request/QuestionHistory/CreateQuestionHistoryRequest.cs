using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.QuestionHistory
{
    public class CreateQuestionHistoryRequest
    {
        public Guid HistoryTestId { get; set; }
        public Guid QuestionId { get; set; }
        public string? Answer { get; set; }
        public string? YourAnswer { get; set; }
    }
}
