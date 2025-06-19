using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.BookTopic;

namespace MathExamGenerator.Service.Interface
{
    public interface IBookTopicService
    {
        Task<BaseResponse<IPaginate<GetBookTopicResponse>>> GetAllBookTopic(int page, int size);
    }
}
