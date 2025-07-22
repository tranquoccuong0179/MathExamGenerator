using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.ExamExchange
{
    public class GetExamExchangeStaffResponse : GetExamExchangeResponse
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
    }
}
