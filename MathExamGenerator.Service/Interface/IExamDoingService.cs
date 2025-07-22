using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamDoing;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Payload.Response.ExamDoing;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Enum;

namespace MathExamGenerator.Service.Interface
{
    public interface IExamDoingService
    {
        Task<BaseResponse<IPaginate<ExamDoingOverviewResponse>>> GetAll(int page, int size);
        Task<BaseResponse<GetExamDoingResponse>> GetById(Guid id);
        Task<BaseResponse<CreateExamDoingResponse>> Create(CreateExamDoingRequest request);
        Task<BaseResponse<bool>> Update(Guid id, UpdateExamDoingRequest request, ExamDoingEnum? status);
        Task<BaseResponse<bool>> Delete(Guid id);
        Task<BaseResponse<List<GetQuestionHistoryResponse>>> GetQuestionHistoriesByTestId(Guid id);
    }
}
