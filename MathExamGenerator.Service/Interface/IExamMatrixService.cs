using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamMatrix;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IExamMatrixService
    {
        Task<BaseResponse<GetExamMatrixResponse>> CreateExamMatrix(CreateExamMatrixWithStructureRequest request);
        Task<BaseResponse<IPaginate<GetExamMatrixResponse>>> GetAllExamMatrix(int page, int size);
        Task<BaseResponse<GetExamMatrixResponse>> GetById(Guid id);
        Task<BaseResponse<bool>> UpdateExamMatrix(Guid id, UpdateExamMatrixRequest request);
        Task<BaseResponse<bool>> DeleteExamMatrix(Guid id);
        Task<BaseResponse<ExamMatrixStructureResponse>> GetMatrixStructure(Guid id);
    }
}
