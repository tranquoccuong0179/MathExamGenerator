using AutoMapper;
using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Response.Analytics;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class AnalyticsService : BaseService<AnalyticsService>, IAnalyticsService
    {
        private readonly string _serviceAccountKeyPath;
        private readonly string _propertyId;

        public AnalyticsService(
            IUnitOfWork<MathExamGeneratorContext> unitOfWork,
            ILogger<AnalyticsService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _serviceAccountKeyPath = Path.Combine(AppContext.BaseDirectory, configuration["GoogleAnalytics:ServiceAccountKeyPath"]);
            _propertyId = configuration["GoogleAnalytics:PropertyId"];
        }

        public async Task<BaseResponse<AnalyticsUsersResponse>> GetUsers()
        {
            try
            {
                var credential = GoogleCredential.FromFile(_serviceAccountKeyPath)
                    .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

                var client = new BetaAnalyticsDataClientBuilder { Credential = credential }.Build();

                var request = new RunReportRequest
                {
                    Property = $"properties/{_propertyId}",
                    DateRanges = { new DateRange { StartDate = "7daysAgo", EndDate = "today" } },
                    Metrics =
            {
                new Metric { Name = "activeUsers" },
                new Metric { Name = "sessions" }
            }
                };

                var response = await client.RunReportAsync(request);
                var data = new AnalyticsUsersResponse
                {
                    Users = response?.Rows[0].MetricValues[0].Value ?? "0",
                    Sessions = response?.Rows[0].MetricValues[1].Value ?? "0"
                };

                return new BaseResponse<AnalyticsUsersResponse>
                {
                    Status = StatusCodes.Status200OK.ToString(),
                    Message = "Lấy dữ liệu người dùng trong 7 ngày",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting GA users");
                return new BaseResponse<AnalyticsUsersResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Lỗi khi lấy dữ liệu người dùng",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<string>> GetRealtimeUsers()
        {
            try
            {
                var credential = GoogleCredential.FromFile(_serviceAccountKeyPath)
                    .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

                var client = new BetaAnalyticsDataClientBuilder { Credential = credential }.Build();

                var request = new RunRealtimeReportRequest
                {
                    Property = $"properties/{_propertyId}",
                    Metrics = { new Metric { Name = "activeUsers" } }
                };

                var response = await client.RunRealtimeReportAsync(request);

                string result = "0";
                if (response?.Rows != null && response.Rows.Count > 0 && response.Rows[0].MetricValues.Count > 0)
                {
                    result = response.Rows[0].MetricValues[0].Value;
                }

                return new BaseResponse<string>
                {
                    Status = StatusCodes.Status200OK.ToString(),
                    Message = "Lấy dữ liệu người dùng đang hoạt động",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting real-time GA users");
                return new BaseResponse<string>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Lỗi khi lấy người dùng",
                    Data = null
                };
            }
        }

    }
}
