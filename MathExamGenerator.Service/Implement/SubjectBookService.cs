using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.SubjectBook;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.SubjectBook;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class SubjectBookService : BaseService<SubjectBookService>, ISubjectBookService
    {
        private readonly IUploadService _uploadService;
        public SubjectBookService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, 
                                  ILogger<SubjectBookService> logger, 
                                  IMapper mapper, 
                                  IHttpContextAccessor httpContextAccessor,
                                  IUploadService uploadService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _uploadService = uploadService;
        }

        public async Task<BaseResponse<CreateSubjectBookResponse>> CreateSubjectBook(CreateSubjectBookRequest request)
        {
            var subjectBooks = await _unitOfWork.GetRepository<SubjectBook>().GetListAsync(
                predicate: s => s.IsActive == true);

            if (subjectBooks.Any(s => s.Title.Equals(request.Title)))
            {
                throw new BadHttpRequestException("Tiêu đề sách môn học đã tồn tại");
            }

            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(request.SubjectId) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy môn học");

            var subjectBook = new SubjectBook
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                FileUrl = await _uploadService.UploadToGoogleDriveAsync(request.FileUrl),
                BookImage = await _uploadService.UploadImage(request.BookImage),
                Description = request.Description,
                SubjectId = request.SubjectId,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime()
            };

            await _unitOfWork.GetRepository<SubjectBook>().InsertAsync(subjectBook);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo sách môn học");
            }

            return new BaseResponse<CreateSubjectBookResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo sách cho môn học thành công",
                Data = new CreateSubjectBookResponse
                {
                    Title = subjectBook.Title,
                    Description = subjectBook.Description,
                    FileUrl = subjectBook.FileUrl,
                    SubjectId = subjectBook.SubjectId,
                    BookImage = subjectBook.BookImage
                }
            };
        }

        public async Task<BaseResponse<bool>> DeleteSubjectBook(Guid id)
        {
            var subjectBook = await _unitOfWork.GetRepository<SubjectBook>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy sách môn học");

            subjectBook.IsActive = false;
            subjectBook.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<SubjectBook>().UpdateAsync(subjectBook);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình xóa sách môn học");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<IPaginate<GetSubjectBookResponse>>> GetAllSubjectBookBySubject(Guid id, int page, int size)
        {
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin môn học");

            var subjectBooks = await _unitOfWork.GetRepository<SubjectBook>().GetPagingListAsync(
                selector: s => new GetSubjectBookResponse
                {
                    Id = s.Id,
                    SubjectId = s.SubjectId,
                    Title = s.Title,
                    Description = s.Description,
                    FileUrl = s.FileUrl,
                    BookImage = s.BookImage
                },
                predicate: s => s.SubjectId.Equals(id) && s.IsActive == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetSubjectBookResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách thông tin sách môn học theo môn học thành công",
                Data = subjectBooks
            };
        }

        public async Task<BaseResponse<IPaginate<GetSubjectBookResponse>>> GetAllSubjectBooks(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                throw new BadHttpRequestException("Số trang và số lượng trong trang phải lớn hơn hoặc bằng 1");
            }

            var subjectBooks = await _unitOfWork.GetRepository<SubjectBook>().GetPagingListAsync(
                selector: s => new GetSubjectBookResponse
                {
                    Id = s.Id,
                    SubjectId = s.SubjectId,
                    Title = s.Title,
                    Description = s.Description,
                    FileUrl = s.FileUrl,
                    BookImage = s.BookImage
                },
                predicate: s => s.IsActive == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetSubjectBookResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách thông tin sách môn học thành công",
                Data = subjectBooks
            };
        }

        public async Task<BaseResponse<GetSubjectBookResponse>> GetSubjectBook(Guid id)
        {
            var subjectBook = await _unitOfWork.GetRepository<SubjectBook>().SingleOrDefaultAsync(
                selector: s => new GetSubjectBookResponse
                {
                    Id = s.Id,
                    SubjectId = s.SubjectId,
                    Title = s.Title,
                    Description = s.Description,
                    FileUrl = s.FileUrl,
                    BookImage = s.BookImage
                },
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin sách này");

            return new BaseResponse<GetSubjectBookResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin sách môn học thành công",
                Data = subjectBook
            };
        }

        public async Task<BaseResponse<GetSubjectBookResponse>> UpdateSubjectBook(Guid id, UpdateSubjectBookRequest request)
        {
            var subjectBook = await _unitOfWork.GetRepository<SubjectBook>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin sách này");

            if (request.SubjectId.HasValue)
            {
                var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                    predicate: s => s.Id.Equals(request.SubjectId) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy môn học");
            }

            subjectBook.Title = request.Title ?? subjectBook.Title;
            subjectBook.Description = request.Description ?? subjectBook.Description;
            subjectBook.SubjectId = request.SubjectId ?? subjectBook.SubjectId;
            subjectBook.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<SubjectBook>().UpdateAsync(subjectBook);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật sách môn học");
            }

            return new BaseResponse<GetSubjectBookResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật thành công",
                Data = new GetSubjectBookResponse
                {
                    Id = subjectBook.Id,
                    Title = subjectBook.Title,
                    Description = subjectBook.Description,
                    FileUrl = subjectBook.FileUrl,
                    BookImage = subjectBook.BookImage,
                    SubjectId = subjectBook.SubjectId
                }
            };
        }
    }
}
