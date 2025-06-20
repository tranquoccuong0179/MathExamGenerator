using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Exam
{
    public class QuestionWithAnswerResponse
    {
        public Guid Id { get; set; }
        public Guid? BookTopicId { get; set; }
        public string? Content { get; set; }
        public string? Level { get; set; }
        public string? Image { get; set; }
        public string? Solution { get; set; }
        public string? CategoryName { get; set; } = default!;
        public string? CategoryGrade { get; set; } = default!;

        public List<AnswerResponse> Answers { get; set; } = new();
    }
}
