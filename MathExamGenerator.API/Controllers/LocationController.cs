
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Location;

namespace MathExamGenerator.API.Controllers
{
    public class LocationController : BaseController<LocationController>
    {
        private readonly ILocationService _locationService;
        public LocationController(ILogger<LocationController> logger, ILocationService locationService) : base(logger)
        {
            _locationService = locationService;
        }

        [HttpGet(ApiEndPointConstant.Location.GetAllLocations)]
        [ProducesResponseType(typeof(BaseResponse<List<GetLocationResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllLocations()
        {
            var response = await _locationService.GetAllLocations();
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
