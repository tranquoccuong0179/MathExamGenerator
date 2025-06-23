using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Comment
{
    public class CreateCommentRequest
    {
        public Guid QuestionId { get; set; }

        public string Content { get; set; } = null!;
    }
}
