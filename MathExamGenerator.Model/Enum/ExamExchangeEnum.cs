using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Enum
{
    public enum ExamExchangeEnum
    {
        [Description("Chờ duyệt")]
        Pending,
        [Description("Đã duyệt")]
        Approved,
        [Description("Từ chối")]
        Rejected,
    }
}
