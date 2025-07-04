using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.Premium;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class PremiumController : BaseController<PremiumController>
    {
        private readonly IPremiumService _premiumService;
        public PremiumController(ILogger<PremiumController> logger, IPremiumService premiumService) : base(logger)
        {
            _premiumService = premiumService;
        }
        [HttpPost(ApiEndPointConstant.Premium.BuyPremium)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> BuyPremium([FromBody] BuyPremiumRequest buyPremiumRequest)
        {
            var response = await _premiumService.BuyPremium(buyPremiumRequest);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
