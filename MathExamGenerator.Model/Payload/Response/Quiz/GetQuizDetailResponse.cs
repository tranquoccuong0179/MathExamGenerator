using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response.Question;

namespace MathExamGenerator.Model.Payload.Response.Quiz
{
    public class GetQuizDetailResponse
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public long? Time { get; set; }

        public int? Quantity { get; set; }

        public DateTime? CreateAt { get; set; }

        public List<QuestionResponse>? Questions { get; set; }
    }
}
