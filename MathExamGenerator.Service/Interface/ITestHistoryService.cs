using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.TestHistory;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Payload.Response.TestHistory;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface ITestHistoryService
    {
        Task<BaseResponse<IPaginate<TestHistoryOverviewResponse>>> GetAll(int page, int size);
        Task<BaseResponse<GetTestHistoryResponse>> GetById(Guid id);
        Task<BaseResponse<CreateTestHistoryResponse>> Create(CreateTestHistoryRequest request);
        Task<BaseResponse<bool>> Update(Guid id, UpdateTestHistoryRequest request);
        Task<BaseResponse<bool>> Delete(Guid id);
        Task<BaseResponse<List<GetQuestionHistoryResponse>>> GetQuestionHistoriesByTestId(Guid id);
    }
}
