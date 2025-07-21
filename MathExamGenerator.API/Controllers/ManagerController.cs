using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.Manager;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers;

public class ManagerController : BaseController<ManagerController>
{
    private readonly IManagerService _managerService;
    public ManagerController(ILogger<ManagerController> logger, IManagerService managerService) : base(logger)
    {
        _managerService = managerService;
    }

    [HttpPost(ApiEndPointConstant.Manager.RegisterManager)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> RegisterManager([FromForm] RegisterManagerRequest request)
    {
        var response = await _managerService.RegisterManager(request);
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpGet(ApiEndPointConstant.Manager.GetAllManager)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status400BadRequest)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllManager([FromQuery] int? page, [FromQuery] int? size)
    {
        var pageNumber = page ?? 1;
        var pageSize = size ?? 10;
        var response = await _managerService.GetAllManager(pageNumber, pageSize);
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpGet(ApiEndPointConstant.Manager.GetManager)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status404NotFound)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> GetManager([FromRoute] Guid id)
    {
        var response = await _managerService.GetManager(id);
        return StatusCode(int.Parse(response.Status), response);
    }
    
    [HttpDelete(ApiEndPointConstant.Manager.DeleteManager)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteManager([FromRoute] Guid id)
    {
        var response = await _managerService.DeleteManager(id);
        return StatusCode(int.Parse(response.Status), response);
    }
}