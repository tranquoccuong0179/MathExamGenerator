using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Wallet;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class WalletService : BaseService<WalletService>, IWalletService
    {
        public WalletService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<WalletService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<WalletResponse>> GetWalletByAccount()
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext) ?? throw new NotFoundException("Chưa đăng nhập tài khoản");

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                 predicate: w => w.AccountId == accountId && w.IsActive ==true,
                 include: w => w.Include(x => x.Account)
                 );

            if (wallet == null)
            {
                return new BaseResponse<WalletResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ví",
                    Data = null
                };
            }

            return new BaseResponse<WalletResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy ví thành công",
                Data = new WalletResponse
                {
                    Id = wallet.Id,
                    Point = wallet.Point.Value,
                    CreateAt = wallet.CreateAt.Value,
                    IsActive = wallet.IsActive.Value,
                }
            };
        }
    }
}
