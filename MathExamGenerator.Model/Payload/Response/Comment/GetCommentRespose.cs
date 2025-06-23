using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Comment
{
    public class GetCommentRespose
    {
        public Guid Id { get; set; }

        public string? Content { get; set; }

        public string? FullName { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
