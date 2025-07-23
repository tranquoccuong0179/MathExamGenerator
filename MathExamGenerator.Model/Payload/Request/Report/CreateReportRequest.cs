using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Report
{
    public class CreateReportRequest
    {
        public Guid? ReportedAccountId { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
    }
}
