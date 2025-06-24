using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.BookChapter;
using MathExamGenerator.Model.Payload.Request.SubjectBook;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Chapter;
using MathExamGenerator.Model.Utils;
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

        public async Task<BaseResponse<CreateChapterReponse>> CreateChapter(CreateBookChapterRequest request)
        {
            var chapters = await _unitOfWork.GetRepository<BookChapter>().GetListAsync(
                predicate: c => c.IsActive == true);

            if (chapters.Any(c => c.Name.Equals(request.Name)))
            {
                throw new BadHttpRequestException("Chapter đã tồn tại");
            }

            var subjectBook = await _unitOfWork.GetRepository<SubjectBook>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(request.SubjectBookId) && s.IsActive == true);

            var chapter = new BookChapter
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ChapterNo = request.ChapterNo,
                SubjectBookId = request.SubjectBookId,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime()
            };

            await _unitOfWork.GetRepository<BookChapter>().InsertAsync(chapter);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo chương");
            }

            return new BaseResponse<CreateChapterReponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo chương thành công",
                Data = new CreateChapterReponse
                {
                    Name = chapter.Name,
                    ChapterNo = chapter.ChapterNo,
                    SubjectBookId = chapter.SubjectBookId
                }
            };
        }

        public async Task<BaseResponse<bool>> DeleteChapter(Guid id)
        {
            var chapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chương");

            chapter.IsActive = false;
            chapter.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<BookChapter>().UpdateAsync(chapter);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình xóa chương");
            }
            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa chương thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetChapterResponse>>> GetAllChapterBySubjectBook(Guid subjectBookId, int page, int size)
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

        public async Task<BaseResponse<IPaginate<GetChapterResponse>>> GetAllChapters(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<BookChapter>().GetPagingListAsync(
                selector: c => new GetChapterResponse
                {
                    Id = c.Id,
                    ChapterNo = c.ChapterNo,
                    Name = c.Name,
                    SubjectBookId = c.SubjectBookId,
                },
                predicate: c => c.IsActive == true,
                orderBy: c => c.OrderBy(c => c.ChapterNo),
                page: page,
                size: size);


            return new BaseResponse<IPaginate<GetChapterResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách thông tin chương thành công",
                Data = response
            };
        }

        public async Task<BaseResponse<GetChapterResponse>> GetChapter(Guid id)
        {
            var response = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                selector: c => new GetChapterResponse
                {
                    Id = c.Id,
                    ChapterNo = c.ChapterNo,
                    Name = c.Name,
                    SubjectBookId = c.SubjectBookId,
                },
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chương sách");

            return new BaseResponse<GetChapterResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin chương sách thành công",
                Data = response
            };
        }

        public async Task<BaseResponse<GetChapterResponse>> UpdateChapter(Guid id, UpdateBookChapterRequest request)
        {
            var chapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(id) && c.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chương");

            if (request.SubjectBookId.HasValue)
            {
                var subjectBook = await _unitOfWork.GetRepository<SubjectBook>().SingleOrDefaultAsync(
                    predicate: s => s.Id.Equals(request.SubjectBookId) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy sách môn học");
            }

            chapter.Name = request.Name ?? chapter.Name;
            chapter.ChapterNo = request.ChapterNo ?? chapter.ChapterNo;
            chapter.SubjectBookId = request.SubjectBookId ?? chapter.SubjectBookId;
            chapter.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<BookChapter>().UpdateAsync(chapter);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật chương");
            }

            return new BaseResponse<GetChapterResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật thành công",
                Data = new GetChapterResponse
                {
                    Id = chapter.Id,
                    Name = chapter.Name,
                    ChapterNo = chapter.ChapterNo,
                    SubjectBookId = chapter.SubjectBookId,
                }
            };
        }
    }
}
