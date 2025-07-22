using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.Category;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Category;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class CategoryService : BaseService<CategoryService>, ICategoryService
    {
        public CategoryService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<CategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<bool>> Create(CategoryRequest request)
        {
            var entity = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Grade = request.Grade,
                IsActive = request.IsActive,
                CreateAt = TimeUtil.GetCurrentSEATime()
            };

            var result = _unitOfWork.GetRepository<Category>().InsertAsync(entity);
            if (result == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Tạo danh mục thất bại"
                };
            }
            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status201Created.ToString(),
                Message = "Tạo danh mục thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Category>();
            var entity = await repo.GetByConditionAsync(c => c.Id == id);
            if (entity == null)
                return new BaseResponse<bool> { Status = StatusCodes.Status404NotFound.ToString(), Message = "Không tìm thấy danh mục" };

            entity.DeleteAt = TimeUtil.GetCurrentSEATime();
            entity.IsActive = false;

            repo.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa danh mục thành công",
                Data = true
            };
        }

        public async Task<BaseResponse<List<CategoryResponse>>> GetAll()
        {
            var result = await _unitOfWork.GetRepository<Category>()
                .GetListAsync(
                    selector: c => new CategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name!,
                        Grade = c.Grade!,
                        IsActive = c.IsActive,
                        CreateAt = c.CreateAt
                    }
                );

            return new BaseResponse<List<CategoryResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy tất cả danh mục thành công",
                Data = result.ToList()
            };
        }


        public async Task<BaseResponse<List<string>>> GetAllGrades()
        {
            var result = await _unitOfWork.GetRepository<Category>()
                .GetListAsync(
                    selector: c => c.Grade!,
                    predicate: c => c.IsActive == true && c.DeleteAt == null
                );

            return new BaseResponse<List<string>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách lớp thành công",
                Data = result.Distinct().ToList()
            };
        }

        public async Task<BaseResponse<List<CategorySelectResponse>>> GetByGrade(string grade)
        {
            var result = await _unitOfWork.GetRepository<Category>()
              .GetListAsync(
                  selector: c => new CategorySelectResponse
                  {
                      Id = c.Id,
                      Name = c.Name!
                  },
                  predicate: c => c.Grade == grade && c.IsActive == true && c.DeleteAt == null
              );
         
            return new BaseResponse<List<CategorySelectResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách danh mục thành công",
                Data = result.ToList()

            };

        }

        public async Task<BaseResponse<CategoryResponse>> GetById(Guid id)
        {
            var result = await _unitOfWork.GetRepository<Category>()
                .SingleOrDefaultAsync(
                    selector: c => new CategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name!,
                        Grade = c.Grade!,
                        IsActive = c.IsActive,
                        CreateAt = c.CreateAt
                    },
                    predicate: c => c.Id == id && c.IsActive == true && c.DeleteAt == null
                );

            if (result == null)
                return new BaseResponse<CategoryResponse> { Status =StatusCodes.Status404NotFound.ToString(), Message = "Không tìm thấy danh mục" };

            return new BaseResponse<CategoryResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh mục thành công",
                Data = result
            };
        }

        public async Task<BaseResponse<bool>> Update(Guid id, CategoryRequest request)
        {
            var repo = _unitOfWork.GetRepository<Category>();
            var entity = await repo.GetByConditionAsync(c => c.Id == id);
            if (entity == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status200OK.ToString(),
                    Message = "Không tìm thấy danh mục"
                };
            }
            entity.Name = request.Name;
            entity.Grade = request.Grade;
            entity.IsActive = request.IsActive;
            entity.UpdateAt = TimeUtil.GetCurrentSEATime();

            repo.UpdateAsync(entity);
            var result =  await _unitOfWork.CommitAsync();
            if (result < 0)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Cập nhật danh mục thất bại"
                };
            }

                return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật danh mục thành công",
                Data = true
            };
        }
    }
}
