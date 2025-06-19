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
        Task<BaseResponse<ExamExchangeResponse>> CreateAsync(ExamExchangeRequest request);
        Task<BaseResponse<ExamExchangeResponse?>> GetAsync(Guid id);
        Task<BaseResponse<IEnumerable<ExamExchangeResponse>>> GetByTeacherAsync(Guid teacherId, string? status);
    }

}
