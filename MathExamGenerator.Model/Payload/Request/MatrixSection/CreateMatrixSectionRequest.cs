using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Request.MatrixSectionDetail;

namespace MathExamGenerator.Model.Payload.Request.MatrixSection
{
    public class CreateMatrixSectionRequest
    {
        public string SectionName { get; set; }
        [Range(1, 1000, ErrorMessage = "Tổng số câu hỏi phải từ 1 đến 1000.")]
        public int TotalQuestions { get; set; }
        public List<CreateMatrixSectionDetailRequest> Details { get; set; }
    }
}
