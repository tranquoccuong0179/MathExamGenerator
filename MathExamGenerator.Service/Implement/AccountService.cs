﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Request.Account;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.User;
using MathExamGenerator.Model.Payload.Settings;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MathExamGenerator.Service.Implement
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IEmailSender _emailSender;
        private readonly IUploadService _uploadService;

        public AccountService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, 
                              ILogger<AccountService> logger, IMapper mapper, 
                              IHttpContextAccessor httpContextAccessor, 
                              IConnectionMultiplexer redis, 
                              IEmailSender emailSender,
                              IUploadService uploadService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _redis = redis;
            _emailSender = emailSender;
            _uploadService = uploadService;
        }

        public async Task<BaseResponse<RegisterResponse>> Register(RegisterRequest request)
        {
            var accountList = await _unitOfWork.GetRepository<Account>().GetListAsync();
            if (accountList.Any(a => a.UserName.Equals(request.UserName)))
            {
                throw new BadHttpRequestException("Tên đăng nhập đã tồn tại");
            }

            if (accountList.Any(a => a.Email.Equals(request.Email)))
            {
                throw new BadHttpRequestException("Email đã tồn tại");
            }

            if (accountList.Any(a => a.Phone.Equals(request.Phone)))
            {
                throw new BadHttpRequestException("Số điện thoại đã tồn tại");
            }

            var redisDb = _redis.GetDatabase();
            if (redisDb == null) throw new RedisServerException("Không thể kết nối tới Redis");

            var key = "emailOtp:" + request.Email;
            var storedOtp = await redisDb.StringGetAsync(key);

            if (string.IsNullOrEmpty(storedOtp))
                throw new NotFoundException("Không tìm thấy mã OTP");
            if (!storedOtp.Equals(request.Otp))
                throw new BadHttpRequestException("Mã OTP không chính xác");

            var account = _mapper.Map<Account>(request);

            account.AvatarUrl = await _uploadService.UploadImage(request.AvatarUrl);

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            

            var wallet = new Wallet()
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                Point = 0,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Wallet>().InsertAsync(wallet);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            
            await redisDb.KeyDeleteAsync(key);

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình đăng ký tài khoản");
            }

            return new BaseResponse<RegisterResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo tài khoản thành công",
                Data = _mapper.Map<RegisterResponse>(account)
            };
        }

        public async Task<BaseResponse<bool>> SendOtp(string email)
        {
            var redisDb = _redis.GetDatabase();

            var key = "emailOtp:" + email;

            var existingOtp = await redisDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(existingOtp))
            {
                return new BaseResponse<bool>()
                {
                    Status = StatusCodes.Status409Conflict.ToString(),
                    Message = "Mã OTP đã được gửi, vui lòng chờ một lát",
                    Data = false
                };
            }

            var accounts = await _unitOfWork.GetRepository<Account>().GetListAsync();

            var otp = OtpUtil.GenerateOtp();

            string html = GetTemplate(email, otp);

            var emailMessage = new EmailMessage()
            {
                ToAddress = email,
                Body = html,
                Subject = otp + " là mã xác thực của bạn",
            };
            await _emailSender.SendEmailAsync(emailMessage);

            var redisSuccess = await redisDb.StringSetAsync(key, otp, TimeSpan.FromMinutes(5));

            if (!redisSuccess)
                throw new BadHttpRequestException("Lỗi khi lưu mã OTP");
            return new BaseResponse<bool>()
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Gửi mã OTP thành công",
                Data = true
            };
        }

        public string GetTemplate(string email, string otp)
        {
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var templatePath = Path.Combine(wwwRootPath, "html", "template.html");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Không tìm thấy template email OTP");

            var templateContent = File.ReadAllText(templatePath);

            templateContent = templateContent.Replace("{otp}", otp);
            templateContent = templateContent.Replace("{email}", email);

            return templateContent;
        }

        public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            if (!account.Password.Equals(PasswordUtil.HashPassword(request.OldPassword)))
            {
                throw new BadHttpRequestException("Mật khẩu cũ không trùng khớp");
            }

            if (!request.NewPassword.Equals(request.ConfirmPassword))
            {
                throw new BadHttpRequestException("Mật khẩu mới và xác nhận mật khẩu không trùng khớp");
            }

            account.Password = PasswordUtil.HashPassword(request.NewPassword);
            account.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình đổi mật khẩu");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Thay đổi mật khẩu thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<bool>> ForgotPassword(string email)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Email.Equals(email) && a.IsActive == true && a.DeleteAt == null) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            var otpResult = await SendOtp(email);

            if (otpResult.Data == false)
            {
                return new BaseResponse<bool>
                {
                    Status = otpResult.Status,
                    Message = otpResult.Message,
                    Data = false
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Gửi mã xác nhận quên mật khẩu thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<GetUserResponse>> VerifyOtp(string email, string otp)
        {
            var redisDb = _redis.GetDatabase();
            if (redisDb == null) throw new RedisServerException("Không thể kết nối tới Redis");

            var key = "emailOtp:" + email;
            var storedOtp = await redisDb.StringGetAsync(key);

            if (string.IsNullOrEmpty(storedOtp))
                throw new NotFoundException("Không tìm thấy mã OTP");
            if (!storedOtp.Equals(otp))
                throw new BadHttpRequestException("Mã OTP không chính xác");

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Email.Equals(email) && a.IsActive == true && a.DeleteAt == null,
                include: a => a.Include(a => a.Wallets)) ?? throw new NotFoundException("Không tìm thấy tài khoản");
            
                await redisDb.KeyDeleteAsync(key);

            return new BaseResponse<GetUserResponse>()
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xác thực thành công",
                Data = new GetUserResponse
                {
                    AccountId = account.Id,
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    FreeTries = account.FreeTries,
                    AvatarUrl = account.AvatarUrl,
                    Point = account.Wallets.FirstOrDefault().Point,
                    IsPremium = account.IsPremium,
                }
            };
        }

        public async Task<BaseResponse<GetUserResponse>> ResetPassword(ResetPasswordRequest request)
        {
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Email.Equals(request.Email) && a.IsActive == true && a.DeleteAt == null,
                include: a => a.Include(a => a.Wallets)) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            if (!request.NewPassword.Equals(request.ConfirmPassword))
            {
                throw new BadHttpRequestException("Mật khẩu mới và xác nhận mật khẩu không khớp");
            }
            
            account.Password = PasswordUtil.HashPassword(request.NewPassword);
            account.UpdateAt = TimeUtil.GetCurrentSEATime();
            
            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            
            var isSuccess = await _unitOfWork.CommitAsync() > 0;
            
            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình đặt lại mật khẩu");
            }

            return new BaseResponse<GetUserResponse>()
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Đặt lại mật khẩu thành công",
                Data = new GetUserResponse()
                {
                    AccountId = account.Id,
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    FreeTries = account.FreeTries,
                    AvatarUrl = account.AvatarUrl,
                    Point = account.Wallets.FirstOrDefault().Point,
                    IsPremium = account.IsPremium,
                }
            };
        }

        public async Task<BaseResponse<GetUserResponse>> ChangeAvatar(IFormFile file)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true,
                include: a => a.Include(a => a.Wallets)) 
                          ?? throw new NotFoundException("Không tìm thấy tài khoản");

            account.AvatarUrl = await _uploadService.UploadImage(file);

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình thay đổi hình nền");
            }

            return new BaseResponse<GetUserResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Thay đổi hình nền thành công",
                Data = new GetUserResponse
                {
                    AccountId = account.Id,
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    DateOfBirth = account.DateOfBirth,
                    Gender = account.Gender,
                    FreeTries = account.FreeTries,
                    AvatarUrl = account.AvatarUrl,
                    Point = account.Wallets.FirstOrDefault().Point,
                    IsPremium = account.IsPremium,
                }
            };
        }
    }
}
