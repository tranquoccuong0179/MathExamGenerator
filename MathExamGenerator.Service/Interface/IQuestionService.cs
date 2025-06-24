using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IQuestionService
    {
        Task<BaseResponse<IPaginate<QuestionResponse>>> GetAllQuestion(int page, int size);
        Task<BaseResponse<IPaginate<QuestionResponse>>> GetQuestionsByTopic(Guid id, int page, int size);
        Task<BaseResponse<string>> DeleteQuestionById(Guid id);


    }
}
