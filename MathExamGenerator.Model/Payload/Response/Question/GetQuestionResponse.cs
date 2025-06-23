using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Question
{
    public class GetQuestionResponse
    {
        public Guid Id { get; set; }

        public string? Level { get; set; }

        public string? Content { get; set; }

        public string? Solution { get; set; }

        public string? Image { get; set; }
    }
}
