using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.QuestionHistory
{
    public class CreateQuestionHistoryResponse
    {
        public Guid Id { get; set; }
        public Guid? HistoryTestId { get; set; }
        public Guid? QuestionId { get; set; }
        public string? Answer { get; set; }
        public string? YourAnswer { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }      
    }
}
