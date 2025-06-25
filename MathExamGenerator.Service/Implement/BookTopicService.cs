using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.BookTopic;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.BookTopic;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class BookTopicService : BaseService<BookTopicService>, IBookTopicService
    {
        public BookTopicService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<BookTopicService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateBookTopicResponse>> CreateBookTopic(CreateBookTopicRequest request)
        {
            var bookTopics = await _unitOfWork.GetRepository<BookTopic>().GetListAsync(
                predicate: b => b.IsActive == true);

            if(bookTopics.Any(b => b.Name.Equals(request.Name)))
            {
                throw new BadHttpRequestException("Tên topic đã tồn tại");
            }

            var bookChapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                predicate: b => b.Id.Equals(request.BookChapterId) && b.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chương");

            var bookTopic = new BookTopic
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                TopicNo = request.TopicNo,
                BookChapterId = bookChapter.Id,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<BookTopic>().InsertAsync(bookTopic);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo chủ đề");
            }

            return new BaseResponse<CreateBookTopicResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo chủ đề thành công",
                Data = new CreateBookTopicResponse
                {
                    Name = bookTopic.Name,
                    TopicNo = bookTopic.TopicNo,
                    BookChapterId = bookTopic.BookChapterId,
                }
            };
        }

        public async Task<BaseResponse<bool>> DeleteBookTopic(Guid id)
        {
            var bookTopic = await _unitOfWork.GetRepository<BookTopic>().SingleOrDefaultAsync(
                predicate: b => b.Id.Equals(id) && b.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chủ đề");

            bookTopic.IsActive = false;
            bookTopic.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<BookTopic>().UpdateAsync(bookTopic);


            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo chủ đề");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetBookTopicResponse>>> GetAllBookTopic(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<BookTopic>().GetPagingListAsync(
                selector: b => new GetBookTopicResponse
                {
                    Id = b.Id,
                    BookChapterId = b.BookChapterId,
                    Name = b.Name,
                    TopicNo = b.TopicNo
                },
                predicate: b => b.IsActive == true,
                orderBy: b => b.OrderBy(c => c.TopicNo),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetBookTopicResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách chủ đề sách thành công",
                Data = response
            };
        }

        public async Task<BaseResponse<IPaginate<GetBookTopicResponse>>> GetAllBookTopicByChapter(Guid id, int page, int size)
        {
            if (page < 1 || size < 1)
            {
                throw new BadHttpRequestException("Số trang và số lượng trong trang phải lớn hơn hoặc bằng 1");
            }

            var bookChapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                predicate: bc => bc.Id.Equals(id) && bc.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chương sách này");

            var response = await _unitOfWork.GetRepository<BookTopic>().GetPagingListAsync(
                selector: b => new GetBookTopicResponse
                {
                    Id = b.Id,
                    BookChapterId = b.BookChapterId,
                    Name = b.Name,
                    TopicNo = b.TopicNo
                },
                predicate: b => b.BookChapterId.Equals(bookChapter.Id) && b.IsActive == true,
                orderBy: b => b.OrderBy(c => c.TopicNo),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetBookTopicResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách chủ đề theo chương thành công",
                Data = response,
            };
        }

        public async Task<BaseResponse<GetBookTopicResponse>> GetBookTopic(Guid id)
        {
            var response = await _unitOfWork.GetRepository<BookTopic>().SingleOrDefaultAsync(
                selector: b => new GetBookTopicResponse
                {
                    Id = b.Id,
                    BookChapterId = b.BookChapterId,
                    Name = b.Name,
                    TopicNo = b.TopicNo
                },
                predicate: b => b.Id.Equals(id) && b.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin chủ đề");

            return new BaseResponse<GetBookTopicResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin chủ đề sách thành công",
                Data = response
            };
        }

        public async Task<BaseResponse<GetBookTopicResponse>> UpdateBookTopic(Guid id, UpdateBookTopicRequest request)
        {
            var bookTopic = await _unitOfWork.GetRepository<BookTopic>().SingleOrDefaultAsync(
                predicate: b => b.Id.Equals(id) && b.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chủ đề");

            if (request.BookChapterId.HasValue)
            {
                var bookChapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                predicate: b => b.Id.Equals(request.BookChapterId) && b.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chương");
            }

            bookTopic.Name = request.Name ?? bookTopic.Name;
            bookTopic.TopicNo = request.TopicNo ?? bookTopic.TopicNo;
            bookTopic.BookChapterId = request.BookChapterId ?? bookTopic.BookChapterId;
            bookTopic.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<BookTopic>().UpdateAsync(bookTopic);


            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật chủ đề");
            }

            return new BaseResponse<GetBookTopicResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật chủ đề thành công",
                Data = new GetBookTopicResponse
                {
                    Id = bookTopic.Id,
                    Name = bookTopic.Name,
                    TopicNo = bookTopic.TopicNo,
                    BookChapterId = bookTopic.BookChapterId,
                }
            };
        }
    }
}
