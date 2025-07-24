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
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Service.Implement
{
    public class ReportService : BaseService<ReportService>, IReportService
    {
        public ReportService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<ReportService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<GetReportResponse>> Create(CreateReportRequest request)
        {

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            if (accountId == null)
            {
                return new BaseResponse<GetReportResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Bạn cần đăng nhập để thực hiện thao tác này",
                };
            }

            if (string.IsNullOrEmpty(request.Type) || !Enum.TryParse<ReportTypeEnum>(request.Type, out var reportType))
            {
                return new BaseResponse<GetReportResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Loại báo cáo không hợp lệ",
                };
            }
            var report = _mapper.Map<Report>(request);
            report.SendAccountId = accountId.Value;
            report.ReportedAccountId = request.ReportedAccountId.Value;
            report.IsActive = true;
            report.Status = ReportStatusEnum.Pending.ToString();

            await _unitOfWork.GetRepository<Report>().InsertAsync(report);
            await _unitOfWork.CommitAsync();

            var result = _mapper.Map<GetReportResponse>(report);
            return new BaseResponse<GetReportResponse>
            {
                Status = StatusCodes.Status201Created.ToString(),
                Data = result,
                Message = "Tạo phiếu báo cáo thành công",
            };
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Report>();
            var report = await repo.SingleOrDefaultAsync(
               predicate: x => x.Id == id && x.IsActive == true
                );

            if (report == null || report.IsActive == false)
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Phiếu báo cáo không tồn tại ",
                };


            report.IsActive = false;
            report.DeleteAt = TimeUtil.GetCurrentSEATime();

            repo.UpdateAsync(report);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Data = true,
                Message = "Xóa phiếu báo cáo thành công",
            };
        }

        public async Task<BaseResponse<IPaginate<GetReportResponse>>> GetAll(int page, int size)
        {
            var repo = _unitOfWork.GetRepository<Report>();
            var reports = await repo.GetPagingListAsync(
                selector: r => _mapper.Map<GetReportResponse>(r),
                predicate: r => r.IsActive == true && r.DeleteAt == null,
                orderBy: q => q.OrderByDescending(r => r.CreateAt),
                page: page,
                size: size
            );

            return new BaseResponse<IPaginate<GetReportResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Data = reports,
                Message = "Lấy danh sách phiếu báo cáo thành công",
            };
        }

        public async Task<BaseResponse<GetReportResponse>> GetById(Guid id)
        {
            var report = await _unitOfWork.GetRepository<Report>()
                .SingleOrDefaultAsync(
                    predicate: r => r.Id == id && r.IsActive == true,
                    include: r => r.Include(r => r.SendAccount).Include(r => r.ReportedAccount)
                );

            if (report == null)
            {
                return new BaseResponse<GetReportResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Phiếu báo cáo không tồn tại ",
                };
             }

            var result = _mapper.Map<GetReportResponse>(report);
          

            return new BaseResponse<GetReportResponse> { Status = StatusCodes.Status200OK.ToString(), Data = result, Message = "Lấy thông tin phiếu thành công" };
        }


        public async Task<BaseResponse<GetReportResponse>> Update(Guid id, ReportStatusEnum status)
        {
            var repo = _unitOfWork.GetRepository<Report>();
            var report = await repo.SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true
            );

            if (report == null)
            {
                return new BaseResponse<GetReportResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy phiếu báo cáo",
                };
            }

            report.Status = status.ToString(); // Enum lưu dạng string trong DB
            report.UpdateAt = TimeUtil.GetCurrentSEATime();

            repo.UpdateAsync(report);
            await _unitOfWork.CommitAsync();

            var result = _mapper.Map<GetReportResponse>(report);
            return new BaseResponse<GetReportResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật trạng thái thành công",
                Data = result
            };
        }


        public async Task<BaseResponse<IPaginate<GetReportResponse>>> GetMyReports(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            if (accountId == null)
            {
                return new BaseResponse<IPaginate<GetReportResponse>>
                {
                    Status = StatusCodes.Status401Unauthorized.ToString(),
                    Message = "Bạn cần đăng nhập để thực hiện thao tác này"
                };
            }

            var repo = _unitOfWork.GetRepository<Report>();
            var reports = await repo.GetPagingListAsync(
                selector: r => _mapper.Map<GetReportResponse>(r),
                predicate: r => r.SendAccountId == accountId && r.IsActive == true && r.DeleteAt == null,
                orderBy: q => q.OrderByDescending(r => r.CreateAt),
                page: page,
                size: size
            );

            return new BaseResponse<IPaginate<GetReportResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Data = reports,
                Message = "Lấy danh sách phiếu báo cáo của bạn thành công"
            };
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
