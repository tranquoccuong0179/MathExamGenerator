using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Report;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Report;
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

namespace MathExamGenerator.Service.Implement
{
    public class ReportService : BaseService<ReportService>, IReportService
    {
        public ReportService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<ReportService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<BaseResponse<GetReportResponse>> Create(CreateReportRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IPaginate<GetReportResponse>>> GetAll(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetReportResponse>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetReportResponse>> Update(Guid id, UpdateReportRequest request)
        {
            throw new NotImplementedException();
        }
        public List<KeyValuePair<string, string>> GetReportTypes()
        {
            return Enum.GetValues(typeof(ReportTypeEnum))
                .Cast<ReportTypeEnum>()
                .Select(e => new KeyValuePair<string, string>(
                    e.ToString(),              
                    e.GetDescriptionFromEnum() 
                )).ToList();
        }
    }
}
