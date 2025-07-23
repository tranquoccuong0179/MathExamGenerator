using MathExamGenerator.Model.Payload.Request.Report;
using MathExamGenerator.Model.Payload.Response.Report;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;

namespace MathExamGenerator.Service.Interface
{
    public interface IReportService
    {
        Task<BaseResponse<GetReportResponse>> Create(CreateReportRequest request);

        Task<BaseResponse<GetReportResponse>> GetById(Guid id);
        Task<BaseResponse<GetReportResponse>> Update(Guid id, ReportStatusEnum request);
        Task<BaseResponse<IPaginate<GetReportResponse>>> GetAll(int page, int size);
        Task<BaseResponse<bool>> Delete(Guid id);
        List<KeyValuePair<string, string>> GetReportTypes();
        Task<BaseResponse<IPaginate<GetReportResponse>>> GetMyReports(int page, int size);


    }
}
