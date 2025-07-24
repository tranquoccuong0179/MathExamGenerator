using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Enum
{
    public enum ReportStatusEnum
    {
        [Description("Chờ xử lý")]
        Pending,

        [Description("Đã xử lý")]
        Resolved,

        [Description("Đã từ chối")]
        Rejected
    }
}
