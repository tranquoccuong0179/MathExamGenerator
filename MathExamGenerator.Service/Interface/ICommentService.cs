using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Request.Comment;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Comment;

namespace MathExamGenerator.Service.Interface
{
    public interface ICommentService
    {
        Task<BaseResponse<CreateCommentResponse>> CreateComment(CreateCommentRequest request);

        Task<BaseResponse<List<GetCommentRespose>>> GetAllCommentByQuestion(Guid id);

        Task<BaseResponse<GetCommentRespose>> UpdateCommnet(Guid id, UpdateCommentRequest request);

        Task<BaseResponse<bool>> DeleteComment(Guid id);
    }
}
