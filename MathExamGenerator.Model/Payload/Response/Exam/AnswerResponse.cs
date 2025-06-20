using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Exam
{
    public class AnswerResponse
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public bool? IsTrue { get; set; }
    }
}
