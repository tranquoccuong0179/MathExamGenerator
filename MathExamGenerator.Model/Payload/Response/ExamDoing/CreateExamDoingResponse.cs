using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.ExamDoing
{
    public class CreateExamDoingResponse
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? ExamId { get; set; }
        public double? Grade { get; set; }
        public string? Status { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<CreateQuestionHistoryResponse> QuestionHistories { get; set; }

    }
}
