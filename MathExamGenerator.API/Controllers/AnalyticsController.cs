using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Analytics;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class AnalyticsController : BaseController<AnalyticsController>
    {
        private readonly IAnalyticsService _analytics;
        public AnalyticsController(ILogger<AnalyticsController> logger, IAnalyticsService analytics) : base(logger)
        {
            _analytics = analytics;
        }

        [HttpGet(ApiEndPointConstant.Analytics.GetUsers)]
        [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _analytics.GetUsers();
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Analytics.GetRealtimeUsers)]
        [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRealtimeUsers()
        {
            var response = await _analytics.GetRealtimeUsers();
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Analytics.GetRegisteredUsersByDay)]
        [ProducesResponseType(typeof(BaseResponse<List<AnalyticsUserDailyResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRegisteredUsersByDay()
        {
            var response = await _analytics.GetRegisteredUsersByDay();
            return StatusCode(int.Parse(response.Status), response);

        }

        [HttpGet(ApiEndPointConstant.Analytics.GetPremiumRevenueByDay)]
        [ProducesResponseType(typeof(BaseResponse<List<AnalyticsRevenueDailyResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPremiumRevenueByDay()
        {
            var response = await _analytics.GetPremiumRevenueByDay();
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
