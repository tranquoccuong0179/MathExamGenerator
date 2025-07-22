using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Enum
{
    public enum ReportTypeEnum
    {
        [Description("Nội dung không phù hợp")]
        InappropriateContent,

        [Description("Ngôn ngữ xúc phạm")]
        OffensiveLanguage,

        [Description("Spam")]
        Spam,
    }
}
