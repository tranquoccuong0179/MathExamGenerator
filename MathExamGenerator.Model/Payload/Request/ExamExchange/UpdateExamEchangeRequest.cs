using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamExchange
{
    public class UpdateExamEchangeRequest
    {
        public Guid CategoryId { get; set; }
        public List<UpdateQuestionRequest> Questions { get; set; }
    }
}
