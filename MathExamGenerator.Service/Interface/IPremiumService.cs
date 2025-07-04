using MathExamGenerator.Model.Payload.Request.Premium;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IPremiumService
    {
        Task<BaseResponse<string>> BuyPremium(BuyPremiumRequest buyPremiumRequest);
    }
}
