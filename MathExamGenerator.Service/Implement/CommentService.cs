using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Request.Comment;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Comment;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class CommentService : BaseService<CommentService>, ICommentService
    {
        public CommentService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<CommentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateCommentResponse>> CreateComment(CreateCommentRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Tài khoản không tồn tại");

            var question = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                predicate: q => q.Id.Equals(request.QuestionId) && q.IsActive == true) ?? throw new NotFoundException("Không tìm thấy câu hỏi");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                QuestionId = request.QuestionId,
                AccountId = accountId,
                Content = request.Content,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Comment>().InsertAsync(comment);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình thêm bình luận cho câu hỏi");
            }

            return new BaseResponse<CreateCommentResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo bình luận thành công",
                Data = new CreateCommentResponse
                {
                    Content = comment.Content,
                    QuestionId = comment.QuestionId,
                    AccountId = comment.AccountId
                }
            };
        }

        public async Task<BaseResponse<bool>> DeleteComment(Guid id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Tài khoản không tồn tại");

            var comment = await _unitOfWork.GetRepository<Comment>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy comment");

            if (comment.AccountId.Equals(account.Id))
            {
                throw new BadHttpRequestException("Bình luận không phải của người dùng");
            }

            comment.IsActive = false;
            comment.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Comment>().UpdateAsync(comment);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình xóa bình luận cho câu hỏi");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa bình luận thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<List<GetCommentRespose>>> GetAllCommentByQuestion(Guid id)
        {
            var question = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                predicate: q => q.Id.Equals(id) && q.IsActive == true) ?? throw new NotFoundException("Không tìm thấy câu hỏi");

            var comments = await _unitOfWork.GetRepository<Comment>().GetListAsync(
                selector: c => new GetCommentRespose
                {
                  Id = c.Id,
                  Content = c.Content,
                  FullName = c.Account.FullName,
                  LastModified = c.UpdateAt,
                },
                predicate: c => c.QuestionId.Equals(id) && c.IsActive == true,
                orderBy: c => c.OrderBy(c => c.UpdateAt),
                include: c => c.Include(c => c.Account));

            return new BaseResponse<List<GetCommentRespose>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách bình luận câu hỏi này thành công",
                Data = comments.ToList()
            };
        }

        public async Task<BaseResponse<GetCommentRespose>> UpdateCommnet(Guid id, UpdateCommentRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Tài khoản không tồn tại");

            var comment = await _unitOfWork.GetRepository<Comment>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy comment");

            if (comment.AccountId.Equals(account.Id))
            {
                throw new BadHttpRequestException("Bình luận không phải của người dùng");
            }

            comment.Content = request.Content ?? comment.Content;
            comment.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Comment>().UpdateAsync(comment);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật bình luận cho câu hỏi");
            }

            return new BaseResponse<GetCommentRespose>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật bình luận thành công",
                Data = new GetCommentRespose
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    FullName = account.FullName,
                    LastModified = comment.UpdateAt,
                }
            };
        }
    }
}
