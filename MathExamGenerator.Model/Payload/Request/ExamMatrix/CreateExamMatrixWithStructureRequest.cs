using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Request.MatrixSection;

namespace MathExamGenerator.Model.Payload.Request.ExamMatrix
{
    public class CreateExamMatrixWithStructureRequest
    {
        public string Name { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public List<CreateMatrixSectionRequest> Sections { get; set; }
    }
}
