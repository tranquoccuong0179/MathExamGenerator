using MathExamGenerator.Model.Payload.Request.MatrixSection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamMatrix
{
    public class UpdateExamMatrixWithStructureRequest
    {
        public string? Name { get; set; }
        public string? Grade { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Tổng số câu hỏi là bắt buộc.")]
        [Range(1, 1000, ErrorMessage = "Tổng số câu hỏi phải từ 1 đến 1000.")]
        public int TotalQuestions { get; set; }

        public List<UpdateMatrixSectionWithDetailRequest> Sections { get; set; } = new();
    }
}
