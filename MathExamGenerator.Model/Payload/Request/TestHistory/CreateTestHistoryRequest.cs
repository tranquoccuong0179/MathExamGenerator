using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.TestHistory
{
    public class CreateTestHistoryRequest
    {
        public Guid? ExamId { get; set; }
        public Guid? QuizId { get; set; }
        [Required(ErrorMessage = "Thời gian bắt đầu không được để trống.")]
        public TimeSpan StartAt { get; set; }
    }
}
