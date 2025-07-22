using MathExamGenerator.Model.Payload.Request.Report;
using MathExamGenerator.Model.Payload.Response.Report;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;

namespace MathExamGenerator.Service.Interface
{
    public interface IReportService
    {
        Task<BaseResponse<GetReportResponse>> Create(CreateReportRequest request);

        Task<BaseResponse<GetReportResponse>> GetById(Guid id);
        Task<BaseResponse<GetReportResponse>> Update(Guid id, UpdateReportRequest request);
        Task<BaseResponse<IPaginate<GetReportResponse>>> GetAll(int page, int size);
        Task<BaseResponse<bool>> Delete(Guid id);
        List<KeyValuePair<string, string>> GetReportTypes();

    }
}
