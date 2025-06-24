using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.MatrixSection;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamMatrix;
using MathExamGenerator.Model.Payload.Response.MatrixSection;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class MatrixSectionService : BaseService<MatrixSectionService>, IMatrixSectionService
    {
        public MatrixSectionService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<MatrixSectionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor) { }

        public async Task<BaseResponse<bool>> DeleteSection(Guid id)
        {
            var section = await _unitOfWork.GetRepository<MatrixSection>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (section == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy section để xoá.",
                    Data = false
                };
            }

            var examUsed = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: e => e.ExamMatrixId == section.ExamMatrixId && e.IsActive == true);

            if (examUsed != null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Không thể xoá section vì ma trận đã được sử dụng để tạo đề thi.",
                    Data = false
                };
            }

            section.IsActive = false;
            section.DeleteAt = TimeUtil.GetCurrentSEATime();
            section.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<MatrixSection>().UpdateAsync(section);

            var details = await _unitOfWork.GetRepository<MatrixSectionDetail>().GetListAsync(
                    predicate: d => d.MatrixSectionId == section.Id && d.IsActive == true);

            foreach (var detail in details)
            {
                detail.IsActive = false;
                detail.UpdateAt = TimeUtil.GetCurrentSEATime();
                detail.DeleteAt = TimeUtil.GetCurrentSEATime();
                _unitOfWork.GetRepository<MatrixSectionDetail>().UpdateAsync(detail);
            }

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Xoá section thất bại.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xoá section thành công.",
                Data = isSuccessfully
            };
        }

        public async Task<BaseResponse<IPaginate<MatrixSectionStructureResponse>>> GetAll(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<MatrixSectionStructureResponse>>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Trang hoặc kích thước không hợp lệ.",
                    Data = null
                };
            }

            var sections = await _unitOfWork.GetRepository<MatrixSection>().GetPagingListAsync(
                selector: x => _mapper.Map<MatrixSectionStructureResponse>(x),
                predicate: x => x.IsActive == true,
                orderBy: o => o.OrderByDescending(x => x.CreateAt),
                include: x => x.Include(s => s.MatrixSectionDetails),
                page: page,
                size: size
            );

            return new BaseResponse<IPaginate<MatrixSectionStructureResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy tất cả section thành công.",
                Data = sections
            };
        }

        public async Task<BaseResponse<MatrixSectionStructureResponse>> GetById(Guid id)
        {
            var section = await _unitOfWork.GetRepository<MatrixSection>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: x => x.Include(x => x.MatrixSectionDetails));

            if (section == null)
            {
                return new BaseResponse<MatrixSectionStructureResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy section.",
                    Data = null
                };
            }

            return new BaseResponse<MatrixSectionStructureResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy section thành công.",
                Data = _mapper.Map<MatrixSectionStructureResponse>(section)
            };
        }

        public async Task<BaseResponse<List<MatrixSectionStructureResponse>>> GetSectionsByMatrixId(Guid matrixId)
        {
            var matrix = await _unitOfWork.GetRepository<ExamMatrix>().SingleOrDefaultAsync(
                predicate: x => x.Id == matrixId && x.IsActive == true);

            if (matrix == null)
            {
                return new BaseResponse<List<MatrixSectionStructureResponse>>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ma trận đề thi.",
                    Data = null
                };
            }

            var sections = await _unitOfWork.GetRepository<MatrixSection>().GetListAsync(
                predicate: s => s.ExamMatrixId == matrixId && s.IsActive == true,
                include: s => s.Include(x => x.MatrixSectionDetails));

            var data = _mapper.Map<List<MatrixSectionStructureResponse>>(sections);

            return new BaseResponse<List<MatrixSectionStructureResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách section thành công.",
                Data = data
            };
        }     
    }
}
