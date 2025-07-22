using MathExamGenerator.API.constant;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class ReportController : BaseController<ReportController>
    {
        private readonly IReportService _reportService;
        public ReportController(ILogger<ReportController> logger, IReportService reportService) : base(logger)
        {
            _reportService = reportService;
        }

        [HttpGet(ApiEndPointConstant.Report.GetReportType)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetReportTypes()
        {
            var reportTypes = _reportService.GetReportTypes();
            return Ok(reportTypes);

        }
    }
}
