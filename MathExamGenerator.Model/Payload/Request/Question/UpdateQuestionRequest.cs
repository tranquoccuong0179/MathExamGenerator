using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Request.Answer;

namespace MathExamGenerator.Model.Payload.Request.Question
{
    public class UpdateQuestionRequest
    {
        public Guid? Id { get; set; }

        public string Content { get; set; }

        public string Level { get; set; }

        public string Solution { get; set; }

        public string Image { get; set; }

        public Guid? BookTopicId { get; set; }
        /// <summary>Danh sách đáp án của câu hỏi.</summary>
        public List<UpdateAnswerRequest> Answers { get; set; }
    }
}
