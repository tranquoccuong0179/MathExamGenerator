using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Staff;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Staff;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers;

public class StaffController : BaseController<StaffController>
{
    private readonly IStaffService _staffService;
    public StaffController(ILogger<StaffController> logger, IStaffService staffService) : base(logger)
    {
        _staffService = staffService;
    }
    
    [HttpPost(ApiEndPointConstant.Staff.RegisterStaff)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> Register([FromForm] RegisterStaffRequest request)
    {
        var response = await _staffService.RegisterStaff(request);
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpGet(ApiEndPointConstant.Staff.GetAllStaff)]
    [ProducesResponseType(typeof(BaseResponse<IPaginate<GetStaffResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<IPaginate<GetStaffResponse>>), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllStaff([FromQuery] int? page, [FromQuery] int? size)
    {
        int pageNumber = page ?? 1;
        int pageSize = size ?? 10;
        var response = await _staffService.GetAllStaff(pageNumber, pageSize);
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpGet(ApiEndPointConstant.Staff.GetStaffById)]
    [ProducesResponseType(typeof(BaseResponse<GetStaffResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<GetStaffResponse>), StatusCodes.Status404NotFound)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetStaffById([FromRoute] Guid id)
    {
        var response = await _staffService.GetStaff(id);
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpGet(ApiEndPointConstant.Staff.GetProfile)]
    [ProducesResponseType(typeof(BaseResponse<GetStaffResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<GetStaffResponse>), StatusCodes.Status404NotFound)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetProfile()
    {
        var response = await _staffService.GetProfileStaff();
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpPut(ApiEndPointConstant.Staff.UpdateStaff)]
    [ProducesResponseType(typeof(BaseResponse<GetStaffResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<GetStaffResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<GetStaffResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffRequest request)
    {
        var response = await _staffService.UpdateStaff(request);
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpDelete(ApiEndPointConstant.Staff.DeleteStaff)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status500InternalServerError)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteStaff([FromRoute] Guid id)
    {
        var response = await _staffService.DeleteStaff(id);
        return StatusCode(int.Parse(response.Status), response);
    }
}