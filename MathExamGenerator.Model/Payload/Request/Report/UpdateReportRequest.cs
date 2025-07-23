using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Report
{
    public class UpdateReportRequest
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
    }
}
