using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.BookTopic;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.BookTopic;

namespace MathExamGenerator.Service.Interface
{
    public interface IBookTopicService
    {
        Task<BaseResponse<CreateBookTopicResponse>> CreateBookTopic(CreateBookTopicRequest request);

        Task<BaseResponse<IPaginate<GetBookTopicResponse>>> GetAllBookTopic(int page, int size);

        Task<BaseResponse<GetBookTopicResponse>> GetBookTopic(Guid id);

        Task<BaseResponse<IPaginate<GetBookTopicResponse>>> GetAllBookTopicByChapter(Guid id, int page, int size);

        Task<BaseResponse<GetBookTopicResponse>> UpdateBookTopic(Guid id, UpdateBookTopicRequest request);

        Task<BaseResponse<bool>> DeleteBookTopic(Guid id);
    }
}
