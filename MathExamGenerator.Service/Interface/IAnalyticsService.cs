using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IAnalyticsService
    {
        Task<BaseResponse<AnalyticsUsersResponse>> GetUsers();
        Task<BaseResponse<string>> GetRealtimeUsers();
        Task<BaseResponse<List<AnalyticsUserDailyResponse>>> GetRegisteredUsersByDay();
        Task<BaseResponse<List<AnalyticsRevenueDailyResponse>>> GetPremiumRevenueByDay(); 

    }
}
