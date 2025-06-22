using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.QuestionHistory
{
    public class GetQuestionHistoryResponse
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string? Answer { get; set; }
        public string? YourAnswer { get; set; }
    }
}
