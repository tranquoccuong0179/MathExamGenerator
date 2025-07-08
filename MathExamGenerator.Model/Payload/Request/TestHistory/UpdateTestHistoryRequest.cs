using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.TestHistory
{
    public class UpdateTestHistoryRequest
    {
        public Guid? ExamId { get; set; }
        public Guid? QuizId { get; set; }
        public TimeSpan? StartAt { get; set; }
        public List<UpdateQuestionHistoryRequest> QuestionHistories { get; set; }
    }
}
