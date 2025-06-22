using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.LikeComment;

namespace MathExamGenerator.Service.Interface
{
    public interface ILikeCommentService
    {
        Task<BaseResponse<LikeCommentResponse>> ToggleLike(Guid id);
    }
}
