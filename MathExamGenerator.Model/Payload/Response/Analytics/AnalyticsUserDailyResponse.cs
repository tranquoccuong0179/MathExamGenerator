using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Analytics
{
    public class AnalyticsUserDailyResponse
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
