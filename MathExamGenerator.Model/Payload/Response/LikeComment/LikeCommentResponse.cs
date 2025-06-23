using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.LikeComment
{
    public class LikeCommentResponse
    {
        public bool Liked { get; set; }

        public int TotalLikes { get; set; }

        public Guid? CommentId { get; set; }
    }
}
