using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.User;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.User;

namespace MathExamGenerator.Service.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<IPaginate<GetUserResponse>>> GetAllUsers(int page, int size);

        Task<BaseResponse<GetUserResponse>> GetUserProfile();

        Task<BaseResponse<GetUserResponse>> GetUser(Guid id);

        Task<BaseResponse<bool>> DeleteUser(Guid id);

        Task<BaseResponse<GetUserResponse>> UpdateUser(UpdateUserRequest request);
    }
}
