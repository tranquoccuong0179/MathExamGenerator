using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamDoing
{
    public class CreateExamDoingRequest
    {
        public Guid? ExamId { get; set; }
        [Required(ErrorMessage = "Thời gian bắt đầu không được để trống.")]
        public TimeSpan Duration { get; set; }
    }
}
