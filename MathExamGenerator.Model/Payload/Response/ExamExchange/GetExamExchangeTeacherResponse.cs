using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.ExamExchange
{
    public class GetExamExchangeTeacherResponse : GetExamExchangeResponse
    {
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
    }
}
