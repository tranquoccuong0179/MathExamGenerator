using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Question
{
    public class QuestionSolutionResponse
    {
        public string QuestionId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Topic { get; set; }
        public string Grade { get; set; }
        public string Chapter { get; set; }
    }
}
