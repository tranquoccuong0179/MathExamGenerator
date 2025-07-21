using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Manager;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Manager;

namespace MathExamGenerator.Service.Interface;

public interface IManagerService
{
    Task<BaseResponse<RegisterResponse>> RegisterManager(RegisterManagerRequest request);

    Task<BaseResponse<IPaginate<GetManagerResponse>>> GetAllManager(int page, int size);

    Task<BaseResponse<GetManagerResponse>> GetManager(Guid id);
    
    Task<BaseResponse<bool>> DeleteManager(Guid id);
}