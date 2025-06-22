using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Comment
{
    public class CreateCommentResponse
    {
        public string? Content { get; set; }

        public Guid? QuestionId { get; set; }

        public Guid? AccountId { get; set; }
    }
}
