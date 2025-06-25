using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.LikeComment;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class LikeCommentService : BaseService<LikeCommentService>, ILikeCommentService
    {
        public LikeCommentService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<LikeCommentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<LikeCommentResponse>> ToggleLike(Guid id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            var comment = await _unitOfWork.GetRepository<Comment>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy bình luận");

            var userInfo = await _unitOfWork.GetRepository<UserInfo>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(comment.AccountId) && u.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin người dùng bình luận câu hỏi");

            var existedLike = await _unitOfWork.GetRepository<LikeComment>().SingleOrDefaultAsync(
                predicate: l => l.CommentId.Equals(comment.Id) && l.AccountId.Equals(accountId));

            bool liked;

            if (existedLike != null)
            {
                _unitOfWork.GetRepository<LikeComment>().DeleteAsync(existedLike);
                liked = false;
            }
            else
            {
                var likeComment = new LikeComment
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    CommentId = comment.Id,
                    IsActive = true,
                    CreateAt = TimeUtil.GetCurrentSEATime()
                };
                await _unitOfWork.GetRepository<LikeComment>().InsertAsync(likeComment);
                liked = true;
            }

            if (liked)
            {
                userInfo.Point += 1;
                userInfo.UpdateAt = TimeUtil.GetCurrentSEATime();
            }
            else
            {
                userInfo.Point -= 1;
                userInfo.UpdateAt = TimeUtil.GetCurrentSEATime();
            }

            _unitOfWork.GetRepository<UserInfo>().UpdateAsync(userInfo);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình thao tác thích bình luận");
            }

            var likeComments = await _unitOfWork.GetRepository<LikeComment>().GetListAsync(
                predicate: l => l.CommentId.Equals(comment.Id));

            var totalLikes = likeComments.Count;

            return new BaseResponse<LikeCommentResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Thích hoặc hủy thích thành công",
                Data = new LikeCommentResponse
                {
                    Liked = liked,
                    TotalLikes = totalLikes,
                    CommentId = comment.Id
                }
            };
        }
    }
}
