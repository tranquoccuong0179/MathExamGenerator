using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Quiz
{
    public class CreateQuizRequest
    {
        public string Name { get; set; } = null!;

        public long Time { get; set; }

        public int Quantity { get; set; }

        public Guid? BookTopicId { get; set; }

        public Guid? BookChapterId { get; set; }
    }
}
