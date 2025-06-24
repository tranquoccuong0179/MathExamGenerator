using MathExamGenerator.Model.Payload.Response.Wallet;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IWalletService
    {
        Task<BaseResponse<WalletResponse>> GetWalletByAccount();
    }
}
