using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Teacher;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Teacher;

namespace MathExamGenerator.Service.Interface
{
    public interface ITeacherService
    {
        Task<BaseResponse<RegisterTeacherResponse>> RegisterTeacher(RegisterTeacherRequest request);

        Task<BaseResponse<IPaginate<GetTeacherResponse>>> GetAllTeacher(int page, int size);

        Task<BaseResponse<bool>> DeleteTeacher(Guid id);

        Task<BaseResponse<GetTeacherResponse>> GetTeacher(Guid id);

        Task<BaseResponse<GetTeacherResponse>> UpdateTeacher(UpdateTeacherRequest request);
    }
}
