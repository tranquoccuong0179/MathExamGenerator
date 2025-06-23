using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.QuestionHistory
{
    public class UpdateQuestionHistoryRequest
    {
        [Required]
        public Guid Id { get; set; }
        public string? YourAnswer { get; set; }
    }
}
