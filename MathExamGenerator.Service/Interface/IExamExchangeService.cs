using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamExchange;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IExamExchangeService
    {
        Task<BaseResponse<ExamExchangeResponse>> Create(ExamExchangeRequest request);
        Task<BaseResponse<bool>> Delete(Guid id);
        Task<BaseResponse<IPaginate<GetExamExchangeResponse>>> GetByStaff(int page, int size);
        Task<BaseResponse<ExamExchangeResponse>> GetExamChange(Guid examchangeId);
        Task<BaseResponse<ExamExchangeResponse>> Update(Guid examchange, UpdateExamEchangeRequest request);
        Task<BaseResponse<IPaginate<GetExamExchangeStaffResponse>>> GetAllStaff(int page, int size);
        Task<BaseResponse<bool>> ApproveExamExchange(Guid id, ExamExchangeEnum? status);

    }

}
