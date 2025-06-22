using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamMatrix
{
    public class UpdateExamMatrixRequest
    {
        public string? Name { get; set; }
        public string? Grade { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
