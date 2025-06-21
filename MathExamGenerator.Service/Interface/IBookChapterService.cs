using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.BookChapter;
using MathExamGenerator.Model.Payload.Request.SubjectBook;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Chapter;

namespace MathExamGenerator.Service.Interface
{
    public interface IBookChapterService
    {
        Task<BaseResponse<CreateChapterReponse>> CreateChapter(CreateBookChapterRequest request);

        Task<BaseResponse<IPaginate<GetChapterResponse>>> GetAllChapters(int page, int size);

        Task<BaseResponse<GetChapterResponse>> GetChapter(Guid id);

        Task<BaseResponse<IPaginate<GetChapterResponse>>> GetAllChapterBySubjectBook(Guid subjectBookId, int page, int size);

        Task<BaseResponse<GetChapterResponse>> UpdateChapter(Guid id, UpdateBookChapterRequest request);

        Task<BaseResponse<bool>> DeleteChapter(Guid id);
    }
}
