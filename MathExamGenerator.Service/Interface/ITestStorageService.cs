using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.TestStorage;
using MathExamGenerator.Model.Payload.Response.TestStorage;
using MathExamGenerator.Model.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface ITestStorageService
    {
        Task<BaseResponse<IPaginate<GetTestStorageResponse>>> GetAll(int page, int size);
        Task<BaseResponse<GetTestStorageResponse>> GetById(Guid id);
        Task<BaseResponse<bool>> Update(Guid id, UpdateTestStorageRequest request);
        Task<BaseResponse<bool>> Delete(Guid id);
        Task<BaseResponse<GetTestStorageResponse>> Create(CreateTestStorageRequest request);
    }

}
