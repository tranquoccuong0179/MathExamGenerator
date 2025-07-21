using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Request.Reply;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Reply;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class ReplyService : BaseService<ReplyService>, IReplyService
    {
        public ReplyService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<ReplyService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateReplyResponse>> CreateReply(Guid id, CreateReplyRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            var comment = await _unitOfWork.GetRepository<Comment>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy bình luận");

            var reply = new Reply
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                AccountId = accountId,
                CommentId = comment.Id,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Reply>().InsertAsync(reply);
            
            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo phản hồi bình luận");
            }

            return new BaseResponse<CreateReplyResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Phản hồi bình luận thành công",
                Data = new CreateReplyResponse
                {
                    AccountId = account.Id,
                    CommentId = comment.Id,
                    Content = reply.Content,
                }
            };
        }

        public async Task<BaseResponse<bool>> DeleteReply(Guid id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            var reply = await _unitOfWork.GetRepository<Reply>().SingleOrDefaultAsync(
                predicate: r => r.Id.Equals(id) && r.IsActive == true) ?? throw new NotFoundException("Không tìm thấy phản hồi bình luận");

            if (!reply.AccountId.Equals(accountId))
            {
                throw new BadHttpRequestException("Phản hồi này không phải của bạn");
            }

            reply.IsActive = false;
            reply.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Reply>().UpdateAsync(reply);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình xóa phản hồi bình luận");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa phản hồi bình luận thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<List<GetReplyResponse>>> GetAllReplyByComment(Guid id)
        {
            var comment = await _unitOfWork.GetRepository<Comment>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy bình luận");

            var replies = await _unitOfWork.GetRepository<Reply>().GetListAsync(
                selector: r => new GetReplyResponse
                {
                    Id = r.Id,
                    Content = r.Content,
                    FullName = r.Account.FullName,
                    LastModified = r.UpdateAt
                },
                predicate: r => r.CommentId.Equals(comment.Id) && r.IsActive == true,
                orderBy: r => r.OrderBy(r => r.UpdateAt),
                include: r => r.Include(r => r.Account));

            return new BaseResponse<List<GetReplyResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách phản hồi bình luận thành công",
                Data = replies.ToList()
            };
        }

        public async Task<BaseResponse<GetReplyResponse>> UpdateReply(Guid id, UpdateReplyRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            var reply = await _unitOfWork.GetRepository<Reply>().SingleOrDefaultAsync(
                predicate: r => r.Id.Equals(id) && r.IsActive == true) ?? throw new NotFoundException("Không tìm thấy phản hồi bình luận");

            if (!reply.AccountId.Equals(accountId))
            {
                throw new BadHttpRequestException("Phản hồi này không phải của bạn");
            }

            reply.Content = request.Content ?? reply.Content;
            reply.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Reply>().UpdateAsync(reply);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật phản hồi bình luận");
            }

            return new BaseResponse<GetReplyResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa phản hồi bình luận thành công",
                Data = new GetReplyResponse
                {
                    Id = reply.Id,
                    Content = reply.Content,
                    FullName = account.FullName,
                    LastModified = reply.UpdateAt
                }
            };
        }
    }
}
