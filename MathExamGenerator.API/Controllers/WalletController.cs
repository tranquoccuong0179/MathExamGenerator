using MathExamGenerator.API.constant;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class WalletController : BaseController<WalletController>
    {
        private readonly IWalletService _walletService;

        public WalletController(ILogger<WalletController> logger, IWalletService walletService) : base(logger)
        {
            _walletService = walletService;
        }

        [HttpGet(ApiEndPointConstant.Wallet.GetWalletByAccount)]
        public async Task<IActionResult> GetWalletByAccount()
        {
            var response = await _walletService.GetWalletByAccount();
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
