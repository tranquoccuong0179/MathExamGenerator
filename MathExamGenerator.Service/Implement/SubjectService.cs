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
using MathExamGenerator.Model.Payload.Response.Subject;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class SubjectService : BaseService<SubjectService>, ISubjectService
    {
        public SubjectService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<SubjectService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<IPaginate<GetSubjectResponse>>> GetAllSubjects(int page, int size)
        {
            var subjects = await _unitOfWork.GetRepository<Subject>().GetPagingListAsync(
                selector: s => new GetSubjectResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Name
                },
                predicate: s => s.IsActive == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetSubjectResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách thông tin môn học thành công",
                Data = subjects
            };
        }

        public async Task<BaseResponse<GetSubjectResponse>> GetSubject(Guid id)
        {
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                selector: s => new GetSubjectResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Name
                },
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy thông tin môn học");

            return new BaseResponse<GetSubjectResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin môn học thành công",
                Data = subject
            };
        }
    }
}
