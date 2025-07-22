using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Staff;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Staff;

namespace MathExamGenerator.Service.Interface;

public interface IStaffService
{
    Task<BaseResponse<RegisterResponse>> RegisterStaff(RegisterStaffRequest request);

    Task<BaseResponse<IPaginate<GetStaffResponse>>> GetAllStaff(int page, int size);
    
    Task<BaseResponse<GetStaffResponse>> GetStaff(Guid id);
    
    Task<BaseResponse<bool>> DeleteStaff(Guid id);

    Task<BaseResponse<GetStaffResponse>> GetProfileStaff();

    Task<BaseResponse<GetStaffResponse>> UpdateStaff(UpdateStaffRequest request);
}