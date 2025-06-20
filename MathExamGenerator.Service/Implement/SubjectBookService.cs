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
using MathExamGenerator.Model.Payload.Response.SubjectBook;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class SubjectBookService : BaseService<SubjectBookService>, ISubjectBookService
    {
        public SubjectBookService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<SubjectBookService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
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
                    FileUrl = s.FileUrl
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
            var subjectBooks = await _unitOfWork.GetRepository<SubjectBook>().GetPagingListAsync(
                selector: s => new GetSubjectBookResponse
                {
                    Id = s.Id,
                    SubjectId = s.SubjectId,
                    Title = s.Title,
                    Description = s.Description,
                    FileUrl = s.FileUrl
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
                    FileUrl = s.FileUrl
                },
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin sách này");

            return new BaseResponse<GetSubjectBookResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin sách môn học thành công",
                Data = subjectBook
            };
        }
    }
}
