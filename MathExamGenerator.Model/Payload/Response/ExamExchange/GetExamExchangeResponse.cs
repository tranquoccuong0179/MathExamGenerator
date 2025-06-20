using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.ExamExchange
{
    public class GetExamExchangeResponse
    {
        public Guid ExamExchangeId { get; set; }                

        public string? Status { get; set; } // Trạng thái: Pending / Approved / Rejected

        public DateTime? CreateAt { get; set; }        

        public int QuestionCount { get; set; }         

        public string? CategoryName { get; set; }        

        public string? CategoryGrade { get; set; }       
    }
}
