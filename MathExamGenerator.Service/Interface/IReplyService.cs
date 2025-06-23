using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Request.Reply;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Reply;

namespace MathExamGenerator.Service.Interface
{
    public interface IReplyService
    {
        Task<BaseResponse<CreateReplyResponse>> CreateReply(Guid id, CreateReplyRequest request);

        Task<BaseResponse<List<GetReplyResponse>>> GetAllReplyByComment(Guid id);

        Task<BaseResponse<GetReplyResponse>> UpdateReply(Guid id, UpdateReplyRequest request);

        Task<BaseResponse<bool>> DeleteReply(Guid id);
    }
}
