using MathExamGenerator.Model.Payload.Request.Category;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Interface
{
    public interface ICategoryService
    {
        Task<BaseResponse<List<CategorySelectResponse>>> GetByGrade(string grade);
        Task<BaseResponse<List<string>>> GetAllGrades();
        Task<BaseResponse<CategoryResponse>> GetById(Guid id);
        Task<BaseResponse<List<CategoryResponse>>> GetAll();
        Task<BaseResponse<bool>> Create(CategoryRequest request);
        Task<BaseResponse<bool>> Update(Guid id, CategoryRequest request);
        Task<BaseResponse<bool>> Delete(Guid id);
    }
}
