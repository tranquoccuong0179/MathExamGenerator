using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Transaction;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class TransactionService : BaseService<TransactionService>, ITransactionService
    {
        public TransactionService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<TransactionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<IPaginate<TransactionRespone>>> GetTransaction(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext) ?? throw new NotFoundException("Chưa đăng nhập tài khoản");

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");
            var transactions = await _unitOfWork.GetRepository<Transaction>()
                .GetPagingListAsync(
              predicate: t => t.IsActive == true
                         && (t.Deposit == null || t.Deposit.AccountId == accountId && t.Deposit.IsActive == true),
              include: t => t.Include(x => x.Deposit),
              selector: t => new TransactionRespone
              {
                  Id = t.Id,
                  Amount = t.Amount.Value,
                  CreateAt = t.CreateAt.Value,
                  Type = t.Type.ToString(),
                  Status = t.Status.ToString(),
                  Description = t.Description,
                  DepositDescription = t.Deposit.Description,
              },
              orderBy: t => t.OrderByDescending(x => x.CreateAt),
              page: page,
              size: size
          );

            return new BaseResponse<IPaginate<TransactionRespone>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách giao dịch thành công",
                Data = transactions
            };

        }
    }
}
