using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.MatrixSection;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.MatrixSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IMatrixSectionService
    {
        Task<BaseResponse<List<MatrixSectionStructureResponse>>> GetSectionsByMatrixId(Guid id);
        Task<BaseResponse<MatrixSectionStructureResponse>> GetById(Guid id);
        Task<BaseResponse<IPaginate<MatrixSectionStructureResponse>>> GetAll(int page, int size);
        Task<BaseResponse<bool>> DeleteSection(Guid id);
    }
}
