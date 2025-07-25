﻿using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Request.Payment;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.payOS.Types;
using Net.payOS;
using MathExamGenerator.Model.Payload.Response.Payment;
using StackExchange.Redis;

namespace MathExamGenerator.Service.Implement
{
    public class PaymentService : BaseService<PaymentService>, IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly PayOS _payOS;
        private readonly PayOSSettings _payOSSettings;
        private readonly IConnectionMultiplexer _redis;

        public PaymentService(
            IUnitOfWork<MathExamGeneratorContext> unitOfWork,
            ILogger<PaymentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config,
            IConnectionMultiplexer redis,
            IOptions<PayOSSettings> payOSSettings
        ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _config = config;
            _payOSSettings = payOSSettings.Value;
            _redis = redis;
            _payOS = new PayOS(_payOSSettings.ClientId, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey);
        }

        public async Task<BaseResponse<string>> Create(PaymentRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");
            if (request.Amount < 0.01m)
            {

                throw new BadHttpRequestException("Số tiền phải lớn hơn 0");
            }


            var random = new Random().Next(100, 999); // 3 chữ số
            var baseCode = DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 1_000_000;
            var orderCode = long.Parse($"{baseCode}{random}"); // 9 chữ số chắc chắn

            string description = "";
            long expiredAt = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds();

            // Tạo signature
            var signatureData = new SortedDictionary<string, object>
                        {
                        { "amount", request.Amount },
                        { "cancelUrl", request.CancelUrl },
                        { "description", description },
                        { "expiredAt", expiredAt },
                        { "orderCode", orderCode },
                        { "returnUrl", request.ReturnUrl }
                            };
            var data = string.Join("&", signatureData.Select(p => $"{p.Key}={p.Value}"));
            var signature = PaymentUtil.ComputeHmacSha256(data, _payOSSettings.ChecksumKey);

            // Tạo PaymentData để gửi đến PayOS
            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)request.Amount,
                description: description,
                items: null,
                cancelUrl: request.CancelUrl,
                returnUrl: request.ReturnUrl,
                signature: signature,
                buyerName: account.FullName,
                buyerPhone: account.Phone,
                buyerEmail: account.Email,
                buyerAddress: "VN",
                expiredAt: (int)expiredAt
            );

            var paymentresult = await _payOS.createPaymentLink(paymentData);
            await _redis.GetDatabase().StringSetAsync($"payos:{orderCode}", accountId.ToString(), TimeSpan.FromMinutes(15));
            return new BaseResponse<string>
            {
                Status = StatusCodes.Status201Created.ToString(),
                Message = "Tạo link thanh toán thành công",
                Data = paymentresult.checkoutUrl
            };

        }

   

        public async Task<BaseResponse<string>> HandleWebhook(WebhookNotification notification)
        {
            try
            {
                if (notification.Success && notification.Data.Code == "00")
                {


                    var orderCode = notification.Data.OrderCode;

                    var amount = notification.Data.Amount;

                    var accountIdStr = await _redis.GetDatabase().StringGetAsync($"payos:{orderCode}");

                    if (accountIdStr.IsNullOrEmpty)
                    {
                        throw new Exception($"Không tìm thấy accountId trong Redis cho orderCode: {orderCode}");
                    }
                   
                    string accountIdString = accountIdStr.ToString();


                    if (!Guid.TryParse(accountIdString, out Guid accountId))
                    {
                        throw new Exception("Dữ liệu Redis không đúng định dạng GUID");
                    }




                    var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                            predicate: a => a.AccountId == accountId && a.IsActive == true
                            ) ?? throw new NotFoundException("Không tìm thấy ví");

                    var depositId = Guid.NewGuid();
                    var deposit = new Deposit
                    {
                        Id = depositId,
                        AccountId = accountId,
                        Code = orderCode.ToString(),
                        Description = "Nạp tiền qua PayOS",
                        Amount = amount,
                        IsActive = true,
                        CreateAt = TimeUtil.GetCurrentSEATime(),
                    };
                    await _unitOfWork.GetRepository<Deposit>().InsertAsync(deposit);

                    // Cập nhật số dư ví
                    wallet.Point += (int)amount;
                    wallet.UpdateAt = TimeUtil.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
                    // Tạo giao dịch (Transaction)
                    var transaction = new Model.Entity.Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = wallet.Id,
                        DepositId = deposit.Id,
                        Type = "Nạp Tiền",
                        Description = "Nạp tiền vào ví",
                        Status = "Success",
                        Amount = amount,
                        IsActive = true,
                        CreateAt = TimeUtil.GetCurrentSEATime(),
                    };
                    await _unitOfWork.GetRepository<Model.Entity.Transaction>().InsertAsync(transaction);

                    await _unitOfWork.CommitAsync();

                    return new BaseResponse<string>
                    {
                        Status = StatusCodes.Status200OK.ToString(),
                        Message = "Xử lý webhook và nạp tiền thành công"
                    };
                }
                return new BaseResponse<string>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Webhook không hợp lệ hoặc giao dịch không thành công"
                };

            }
            catch (Exception ex)
            {

                return new BaseResponse<string>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = $"Lỗi khi xử lý webhook: {ex.Message}"
                };
            }

        }

        public async Task<BaseResponse<string>> CreateExamPayment(PaymentExamRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            if (accountId == null)
            {
                throw new BadHttpRequestException("Bạn cần đăng nhập để thực hiện thao tác này");
            }

            var examDoing = await _unitOfWork.GetRepository<ExamDoing>().SingleOrDefaultAsync(
                predicate: e => e.Id == request.ExamDoingId && e.IsActive == true
            );
            if (examDoing == null)
                throw new NotFoundException("Không tìm thấy bài làm");

            var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                predicate: w => w.AccountId == accountId && w.IsActive == true
            ) ?? throw new NotFoundException("Không tìm thấy ví");

            if (request.Amount <= 0)
            {
                throw new BadHttpRequestException("Số tiền thanh toán phải lớn hơn 0");
            }

            if (request.Amount > wallet.Point)
            {
                var shortfall = request.Amount - wallet.Point;
                return new BaseResponse<string>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = $"Số dư ví không đủ. Bạn cần nạp thêm {shortfall} điểm để thực hiện thanh toán.",
                    Data = null
                };
            }

            wallet.Point -= request.Amount;
            wallet.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);

            var transaction = new Model.Entity.Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                ExamDoingId = request.ExamDoingId,
                Amount = request.Amount,
                Type = "Mua Bài",
                Description = "Thanh toán bài làm",
                Status = "Success", 
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime()
            };

            await _unitOfWork.GetRepository<Model.Entity.Transaction>().InsertAsync(transaction);

       

            await _unitOfWork.CommitAsync();

            return new BaseResponse<string>
            {
                Status = StatusCodes.Status201Created.ToString(),
                Message = "Thanh toán bài làm thành công",
                Data = transaction.Id.ToString()
            };
        }


    }
}
