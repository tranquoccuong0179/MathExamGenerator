using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.Premium;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class PremiumService : BaseService<PremiumService>, IPremiumService
    {
        public PremiumService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<PremiumService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<string>> BuyPremium(BuyPremiumRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null) throw new Exception("Không tìm thấy tài khoản");

            if(request.Days != 30 && request.Days != 365)
            {
                return new BaseResponse<string>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Gói Premium không hợp lệ. Vui lòng chọn gói 30 hoặc 365 ngày.",
                    Data = null
                };
            }

            decimal price = request.Days == 30 ? 48000 : 499999;
            var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                predicate: w => w.AccountId.Equals(accountId) && w.IsActive == true);

            if (wallet.Point < price)
            {
                var amount = price - wallet.Point;
                return new BaseResponse<string>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = $"Rất tiếc, bạn cần nạp thêm {amount:N0}đ để mua gói này.",
                    Data = null
                };
            }

            wallet.Point -= (int)price;
            wallet.UpdateAt = TimeUtil.GetCurrentSEATime();
            account.IsPremium = true;
            account.PremiumExpireAt = TimeUtil.GetCurrentSEATime().AddDays(request.Days.Value);

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                DepositId = null,
                Amount = price,
                Description = $"Mua gói Premium {request.Days} ngày",
                Type= "Thanh Toán",
                Status = "Success",
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
            };
            await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
            _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
            _unitOfWork.GetRepository<Account>().UpdateAsync(account);

            await _unitOfWork.CommitAsync();



            return new BaseResponse<string>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = $"Bạn đã mua gói Premium {request.Days} ngày thành công.",
                Data = $"Gói Premium của bạn sẽ hết hạn vào {account.PremiumExpireAt:dd/MM/yyyy}"
            };

        }
    }
}
