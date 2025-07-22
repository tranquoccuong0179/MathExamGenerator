using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Report
{
    public class GetReportResponse
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public Guid? SendAccountId { get; set; }
        public Guid? ReportedAccountId { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
