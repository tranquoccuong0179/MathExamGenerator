using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Quiz;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Quiz;

namespace MathExamGenerator.Service.Interface
{
    public interface IQuizService
    {
        Task<BaseResponse<CreateQuizResponse>> CreateQuiz(CreateQuizRequest request);

        Task<BaseResponse<IPaginate<GetQuizResponse>>> GetAllQuiz(int page, int size);

        Task<BaseResponse<GetQuizDetailResponse>> GetQuizDetail(Guid id);
    }
}
