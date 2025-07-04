using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Request.Account;

namespace MathExamGenerator.Service.Interface
{
    public interface IAccountService
    {
        Task<BaseResponse<RegisterResponse>> Register(RegisterRequest request);

        Task<BaseResponse<RegisterResponse>> RegisterManager(RegisterManagerRequest request);

        Task<BaseResponse<bool>> SendOtp(string email);

        Task<BaseResponse<bool>> ChangePassword(ChangePasswordRequest request);

        Task<BaseResponse<bool>> ForgotPassword(string email);
    }
}
