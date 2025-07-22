using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Package;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface IPackageService
    {
        Task<BaseResponse<GetPackageResponse>> GetById(Guid id);
        Task<BaseResponse<IPaginate<GetPackageResponse>>> GetAll(int page, int size);

        Task<BaseResponse<GetPackageResponse>> Create(CreatePackageRequest request);
        Task<BaseResponse<GetPackageResponse>> Update(Guid id, UpdatePackageRequest request);
        Task<BaseResponse<bool>> Delete(Guid id);
    }
}
