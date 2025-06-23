using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.MatrixSectionDetail
{
    public class UpdateMatrixSectionDetailRequest
    {
        public Guid? BookChapterId { get; set; }
        public Guid? BookTopicId { get; set; }
        public string? Difficulty { get; set; }
        [Required]
        public int QuestionCount { get; set; }
        [Required]
        public double ScorePerQuestion { get; set; }
    }
}
