using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Subject;

namespace MathExamGenerator.Service.Interface
{
    public interface ISubjectService
    {
        Task<BaseResponse<IPaginate<GetSubjectResponse>>> GetAllSubjects(int page, int size);

        Task<BaseResponse<GetSubjectResponse>> GetSubject(Guid id);
    }
}
