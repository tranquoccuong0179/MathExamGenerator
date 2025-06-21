using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.BookTopic
{
    public class CreateBookTopicResponse
    {
        public string? Name { get; set; }

        public int? TopicNo { get; set; }

        public Guid? BookChapterId { get; set; }
    }
}
