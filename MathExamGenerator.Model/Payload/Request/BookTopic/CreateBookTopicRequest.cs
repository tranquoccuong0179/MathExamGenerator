using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.BookTopic
{
    public class CreateBookTopicRequest
    {
        public string Name { get; set; } = null!;

        public int TopicNo { get; set; }

        public Guid BookChapterId { get; set; }
    }
}
