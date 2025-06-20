using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamExchange
{
    public class QuestionRequest
    {
        public Guid? BookTopicId { get; set; }   
        public Guid? BookChapterId { get; set; } 
        public string Level { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string Solution { get; set; } = default!;
        public string? Image { get; set; }
        
        public List<AnswerReQuest> Answers { get; set; } = new();
    }
}
