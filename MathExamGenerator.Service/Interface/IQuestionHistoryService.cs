using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IQuestionHistoryService
    {
        Task<BaseResponse<IPaginate<GetQuestionHistoryResponse>>> GetAll(int page, int size);
        Task<BaseResponse<GetQuestionHistoryResponse>> GetById(Guid id);
        Task<BaseResponse<CreateQuestionHistoryResponse>> Create(CreateQuestionHistoryRequest request);
        Task<BaseResponse<bool>> Update(Guid id, UpdateQuestionHistoryRequest request);
        Task<BaseResponse<bool>> Delete(Guid id);
    }
}
