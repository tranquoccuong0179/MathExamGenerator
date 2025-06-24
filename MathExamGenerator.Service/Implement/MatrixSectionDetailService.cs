using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.MatrixSectionDetail;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Model.Payload.Response.MatrixSectionDetail;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MathExamGenerator.Service.Implement
{
    public class MatrixSectionDetailService : BaseService<MatrixSectionDetailService>, IMatrixSectionDetailService
    {
        public MatrixSectionDetailService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<MatrixSectionDetailService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor): base(unitOfWork, logger, mapper, httpContextAccessor) { }

        public async Task<BaseResponse<bool>> DeleteDetail(Guid id)
        {
            var detail = await _unitOfWork.GetRepository<MatrixSectionDetail>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (detail == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy detail để xoá.",
                    Data = false
                };
            }

            var section = await _unitOfWork.GetRepository<MatrixSection>().SingleOrDefaultAsync(
                predicate: x => x.Id == detail.MatrixSectionId);

            var examUsed = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: e => e.ExamMatrixId == section.ExamMatrixId && e.IsActive == true);

            if (examUsed != null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Không thể xoá detail vì ma trận đã được sử dụng để tạo đề thi.",
                    Data = false
                };
            }

            detail.IsActive = false;
            detail.DeleteAt = TimeUtil.GetCurrentSEATime();
            detail.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<MatrixSectionDetail>().UpdateAsync(detail);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi xảy ra trong quá trình xóa detail.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xoá detail thành công.",
                Data = isSuccessfully
            };
        }

        public async Task<BaseResponse<IPaginate<MatrixSectionDetailResponse>>> GetAll(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<MatrixSectionDetailResponse>>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Trang hoặc kích thước không hợp lệ.",
                    Data = null
                };
            }

            var result = await _unitOfWork.GetRepository<MatrixSectionDetail>().GetPagingListAsync(
                selector: x => _mapper.Map<MatrixSectionDetailResponse>(x),
                page: page,
                size: size,
                orderBy: q => q.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true);

            return new BaseResponse<IPaginate<MatrixSectionDetailResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách detail thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<List<MatrixSectionDetailResponse>>> GetAllBySectionId(Guid id)
        {
            var section = await _unitOfWork.GetRepository<MatrixSection>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (section == null)
            {
                return new BaseResponse<List<MatrixSectionDetailResponse>>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy section."
                };
            }

            var details = await _unitOfWork.GetRepository<MatrixSectionDetail>().GetListAsync(
                predicate: d => d.MatrixSectionId == id && d.IsActive == true);

            var result = details.Select(d => _mapper.Map<MatrixSectionDetailResponse>(d)).ToList();

            return new BaseResponse<List<MatrixSectionDetailResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách detail theo section thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<MatrixSectionDetailResponse>> GetById(Guid id)
        {
            var detail = await _unitOfWork.GetRepository<MatrixSectionDetail>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (detail == null)
            {
                return new BaseResponse<MatrixSectionDetailResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy detail.",
                };
            }

            return new BaseResponse<MatrixSectionDetailResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy detail thành công.",
                Data = _mapper.Map<MatrixSectionDetailResponse>(detail)
            };
        }
    }
}
