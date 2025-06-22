using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Reply
{
    public class CreateReplyResponse
    {
        public string? Content { get; set; }

        public Guid CommentId { get; set; }

        public Guid AccountId { get; set; }
    }
}
