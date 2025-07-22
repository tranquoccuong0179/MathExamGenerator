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
using Google.Apis.Drive.v3.Data;
using MathExamGenerator.Model.Enum;
using System.Transactions;

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

        public async Task<BaseResponse<List<AnalyticsUserDailyResponse>>> GetRegisteredUsersByDay()
        {
            try
            {
                var rawList = await _unitOfWork.GetRepository<Account>()
                    .GetListAsync(
                        selector: g => new
                        {
                            Date = g.CreateAt!.Value.Date
                        },
                        predicate: u => u.CreateAt.HasValue &&
                            (u.Role == RoleEnum.USER.ToString())
                    );

                var grouped = rawList
                    .GroupBy(x => x.Date)
                    .Select(g => new AnalyticsUserDailyResponse
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToList();

                return new BaseResponse<List<AnalyticsUserDailyResponse>>
                {
                    Status = StatusCodes.Status200OK.ToString(),
                    Message = "Lấy thống kê đăng ký người dùng theo ngày thành công",
                    Data = grouped
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Analytics] Lỗi khi thống kê đăng ký người dùng theo ngày");

                return new BaseResponse<List<AnalyticsUserDailyResponse>>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Đã xảy ra lỗi khi xử lý dữ liệu",
                    Data = null
                };
            }
        }



        public async Task<BaseResponse<List<AnalyticsRevenueDailyResponse>>> GetPremiumRevenueByDay()
        {
            try
            {
                var transactions = await _unitOfWork.GetRepository<Model.Entity.Transaction>()
                    .GetListAsync(
                        selector: t => new
                        {
                            Date = t.CreateAt.Value.Date,
                            t.Amount,
                            t.Type,
                            t.Status
                        },
                        predicate: t => t.CreateAt.HasValue &&
                                        t.Type == "Thanh Toán" &&
                                        t.Status == "Success"
                    );

                var grouped = transactions
                    .GroupBy(t => t.Date)
                    .Select(g => new AnalyticsRevenueDailyResponse
                    {
                        Date = g.Key,
                        TotalRevenue = (decimal)g.Sum(x => x.Amount),
                        OrderCount = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToList();

                return new BaseResponse<List<AnalyticsRevenueDailyResponse>>
                {
                    Status = StatusCodes.Status200OK.ToString(),
                    Message = "Thống kê doanh thu premium theo ngày thành công",
                    Data = grouped
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Analytics] Lỗi khi thống kê doanh thu premium");
                return new BaseResponse<List<AnalyticsRevenueDailyResponse>>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Lỗi hệ thống",
                    Data = null
                };
            }
        }

    }
}
