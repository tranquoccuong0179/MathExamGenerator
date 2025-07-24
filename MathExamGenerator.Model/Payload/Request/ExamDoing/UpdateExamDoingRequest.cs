using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamDoing
{
    public class UpdateExamDoingRequest
    {
        public TimeSpan? Duration { get; set; }
        public List<UpdateQuestionHistoryRequest> QuestionHistories { get; set; }
    }
}
