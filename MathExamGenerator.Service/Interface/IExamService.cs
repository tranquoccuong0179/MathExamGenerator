using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Exam;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IExamService
    {
        Task<BaseResponse<IPaginate<GetExamResponse>>> GetAllExam(int page, int size);
        Task<BaseResponse<IPaginate<GetExamResponse>>> GetExamsOfCurrentUser(int page, int size);
        Task<BaseResponse<GetExamResponse>> GetById(Guid id);
        Task<BaseResponse<CreateExamResponse>> CreateExam(CreateExamRequest request);
        Task<BaseResponse<bool>> UpdateExam(Guid id, UpdateExamRequest request, ExamEnum? examEnum);
        Task<BaseResponse<bool>> DeleteExam(Guid id);
        Task<BaseResponse<ExamWithQuestionsResponse>> GetAllQuestionByExam(Guid examId);
    }
}
