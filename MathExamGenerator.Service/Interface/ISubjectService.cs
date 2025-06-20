using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Subject;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Subject;

namespace MathExamGenerator.Service.Interface
{
    public interface ISubjectService
    {
        Task<BaseResponse<CreateSubjectResponse>> CreateSubject(CreateSubjectRequest request);

        Task<BaseResponse<IPaginate<GetSubjectResponse>>> GetAllSubjects(int page, int size);

        Task<BaseResponse<GetSubjectResponse>> GetSubject(Guid id);

        Task<BaseResponse<GetSubjectResponse>> UpdateSubject(Guid id, UpdateSubjectRequest request);

        Task<BaseResponse<bool>> DeleteSubject(Guid id);
    }
}
