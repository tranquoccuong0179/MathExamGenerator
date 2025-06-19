using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Chapter;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class BookChapterService : BaseService<BookChapterService>, IBookChapterService
    {
        public BookChapterService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<BookChapterService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<IPaginate<GetChapterResponse>>> GetAllChapterByBook(Guid subjectBookId, int page, int size)
        {
            var subjectBook = await _unitOfWork.GetRepository<SubjectBook>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(subjectBookId) && s.IsActive == true);

            if (subjectBook == null)
            {
                throw new NotFoundException("Không tìm thấy sách của môn học");
            }

            var response = await _unitOfWork.GetRepository<BookChapter>().GetPagingListAsync(
                selector: c => new GetChapterResponse
                {
                    Id = c.Id,
                    ChapterNo = c.ChapterNo,
                    Name = c.Name,
                    SubjectBookId = c.SubjectBookId,
                },
                predicate: c => c.SubjectBookId.Equals(subjectBookId) && c.IsActive == true,
                orderBy: c => c.OrderBy(c => c.ChapterNo),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetChapterResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách chương theo " + $"{subjectBook.Title}",
                Data = response
            };
        }
    }
}
