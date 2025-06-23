using MathExamGenerator.Model.Payload.Request.MatrixSectionDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.MatrixSection
{
    public class UpdateMatrixSectionWithDetailRequest
    {
        public string SectionName { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public double TotalScore { get; set; }

        public List<UpdateMatrixSectionDetailRequest> Details { get; set; } = new();
    }
}
