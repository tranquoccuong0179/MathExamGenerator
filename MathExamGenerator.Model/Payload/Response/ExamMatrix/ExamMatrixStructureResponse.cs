using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response.MatrixSection;

namespace MathExamGenerator.Model.Payload.Response.ExamMatrix
{
    public class ExamMatrixStructureResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public List<MatrixSectionStructureResponse> MatrixSections { get; set; }
    }
}
