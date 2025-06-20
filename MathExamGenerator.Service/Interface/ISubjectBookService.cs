using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.SubjectBook;

namespace MathExamGenerator.Service.Interface
{
    public interface ISubjectBookService
    {
        Task<BaseResponse<IPaginate<GetSubjectBookResponse>>> GetAllSubjectBooks(int page, int size);

        Task<BaseResponse<GetSubjectBookResponse>> GetSubjectBook(Guid id);

        Task<BaseResponse<IPaginate<GetSubjectBookResponse>>> GetAllSubjectBookBySubject(Guid id, int page, int size);
    }
}
