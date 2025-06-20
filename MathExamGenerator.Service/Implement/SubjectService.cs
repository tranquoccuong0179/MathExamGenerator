using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Subject;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Subject;
using MathExamGenerator.Model.Utils;
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

        public async Task<BaseResponse<CreateSubjectResponse>> CreateSubject(CreateSubjectRequest request)
        {
            var subjects = await _unitOfWork.GetRepository<Subject>().GetListAsync(
                predicate: s => s.IsActive == true);

            if(subjects.Any(s => s.Name.Equals(request.Name)))
            {
                throw new BadHttpRequestException("Tên môn học đã tồn tại");
            }

            if (subjects.Any(s => s.Code.Equals(request.Code)))
            {
                throw new BadHttpRequestException("Mã môn học đã tồn tại");
            }

            var subject = new Subject
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
                UpdateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Subject>().InsertAsync(subject);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo môn học");
            }

            return new BaseResponse<CreateSubjectResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo thành công",
                Data = new CreateSubjectResponse
                {
                    Name = subject.Name,
                    Code = subject.Code
                }
            };
        }

        public async Task<BaseResponse<bool>> DeleteSubject(Guid id)
        {
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy môn học");

            subject.IsActive = false;
            subject.DeleteAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình xóa môn học");
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa thành công",
                Data = true
            };
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

        public async Task<BaseResponse<GetSubjectResponse>> UpdateSubject(Guid id, UpdateSubjectRequest request)
        {
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.IsActive == true) ?? throw new NotFoundException("Không tìm thấy môn học");

            subject.Name = request.Name ?? subject.Name;
            subject.Code = request.Code ?? subject.Code;
            subject.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình cập nhật môn học");
            }

            return new BaseResponse<GetSubjectResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật môn học thành công",
                Data = new GetSubjectResponse
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    Code = subject.Code,
                }
            };
        }
    }
}
