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
        public Guid AccountId { get; set; }
        public Guid? ExamId { get; set; }
        public Guid? QuizId { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu không được để trống.")]
        public TimeOnly StartAt { get; set; }
    }
}
