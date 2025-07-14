using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class AnalyticsController : BaseController<AnalyticsController>
    {
        private readonly IAnalyticsService _analytics;
        public AnalyticsController(ILogger<AnalyticsController> logger , IAnalyticsService analytics) : base(logger)
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

    }
}
