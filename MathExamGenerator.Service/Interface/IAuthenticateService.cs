using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Request.Authentication;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Authentication;

namespace MathExamGenerator.Service.Interface
{
    public interface IAuthenticateService
    {
        Task<BaseResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest request);
    }
}
