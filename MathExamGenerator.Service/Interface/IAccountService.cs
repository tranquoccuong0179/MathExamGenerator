﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Request.Account;
using Microsoft.AspNetCore.Http;
using MathExamGenerator.Model.Payload.Response.User;

namespace MathExamGenerator.Service.Interface
{
    public interface IAccountService
    {
        Task<BaseResponse<RegisterResponse>> Register(RegisterRequest request);
        
        Task<BaseResponse<bool>> SendOtp(string email);

        Task<BaseResponse<bool>> ChangePassword(ChangePasswordRequest request);

        Task<BaseResponse<bool>> ForgotPassword(string email);
        
        Task<BaseResponse<GetUserResponse>> VerifyOtp(string email, string otp);

        Task<BaseResponse<GetUserResponse>> ResetPassword(ResetPasswordRequest request);

        Task<BaseResponse<GetUserResponse>> ChangeAvatar(IFormFile file);
    }
}
