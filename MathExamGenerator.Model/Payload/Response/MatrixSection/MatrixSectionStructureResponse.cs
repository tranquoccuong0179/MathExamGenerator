using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response.MatrixSectionDetail;

namespace MathExamGenerator.Model.Payload.Response.MatrixSection
{
    public class MatrixSectionStructureResponse
    {
        public Guid Id { get; set; }
        public string SectionName { get; set; }
        public int TotalQuestions { get; set; }
        public double TotalScore { get; set; }
        public List<MatrixSectionDetailResponse> MatrixSectionDetails { get; set; }
    }
}
