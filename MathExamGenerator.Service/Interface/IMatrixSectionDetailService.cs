using MathExamGenerator.Model.Payload.Response.MatrixSectionDetail;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.MatrixSectionDetail;

namespace MathExamGenerator.Service.Interface
{
    public interface IMatrixSectionDetailService
    {
        Task<BaseResponse<IPaginate<MatrixSectionDetailResponse>>> GetAll(int page, int size);
        Task<BaseResponse<MatrixSectionDetailResponse>> GetById(Guid id);
        Task<BaseResponse<List<MatrixSectionDetailResponse>>> GetAllBySectionId(Guid id);
        Task<BaseResponse<bool>> UpdateDetail(Guid id, UpdateMatrixSectionDetailRequest request);
        Task<BaseResponse<bool>> DeleteDetail(Guid id);
    }
}
