using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response.ExamExchange;
using MathExamGenerator.Model.Payload.Response.Answer;

namespace MathExamGenerator.Model.Payload.Response.Question
{
    public class QuestionResponse
    {
        public Guid? BookTopicId { get; set; }   
        public Guid? BookTopicId { get; set; }
        public string Level { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string Solution { get; set; } = default!;
        public string? Image { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; } = default!;
        public string? CategoryGrade { get; set; } = default!;
        public List<AnswerResponse> Answers { get; set; } = new();
    }
}
