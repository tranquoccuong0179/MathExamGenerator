using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Chapter;

namespace MathExamGenerator.Service.Interface
{
    public interface IBookChapterService
    {
        Task<BaseResponse<IPaginate<GetChapterResponse>>> GetAllChapterBySubjectBook(Guid subjectBookId, int page, int size);
    }
}
