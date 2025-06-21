using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.MatrixSection
{
    public class UpdateMatrixSectionRequest
    {
        public string? SectionName { get; set; }
        public int? TotalQuestions { get; set; }
        public double? TotalScore { get; set; }
    }
}
