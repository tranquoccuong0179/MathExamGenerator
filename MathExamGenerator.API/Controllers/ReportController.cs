using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Report;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Report;
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
        [HttpPost(ApiEndPointConstant.Report.CreateReport)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
        {
            var response = await _reportService.Create(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Report.GetReportById)]
        [ProducesResponseType(typeof(BaseResponse<GetReportResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var response = await _reportService.GetById(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.Report.UpdateReport)]
        [ProducesResponseType(typeof(BaseResponse<GetReportResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateReport([FromRoute] Guid id, [FromQuery] ReportStatusEnum request)
        {
            var response = await _reportService.Update(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Report.GetAllReports)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetReportResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllReports([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _reportService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpDelete(ApiEndPointConstant.Report.DeleteReport)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteReport([FromRoute] Guid id)
        {
            var response = await _reportService.Delete(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Report.GetMyReports)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetReportResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMyReports([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _reportService.GetMyReports(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);

        }

    }
}
