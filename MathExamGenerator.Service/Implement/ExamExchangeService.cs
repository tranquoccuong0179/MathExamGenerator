using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.ExamExchange;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamExchange;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
namespace MathExamGenerator.Service.Implement
{
    public class ExamExchangeService : BaseService<ExamExchangeService>,IExamExchangeService
    {
        public ExamExchangeService(
           IUnitOfWork<MathExamGeneratorContext> unitOfWork,
           ILogger<ExamExchangeService> logger,
           IMapper mapper,
           IHttpContextAccessor accessor)
           : base(unitOfWork, logger, mapper, accessor)
        {
        }


        public async Task<BaseResponse<ExamExchangeResponse>> CreateAsync(ExamExchangeRequest request)
        {
            // Lấy repo generic
            var examRepo = _unitOfWork.GetRepository<ExamExchange>();
            var categoryRepo = _unitOfWork.GetRepository<Category>();

            // Map Request -> Entity
            var examEntity = _mapper.Map<ExamExchange>(request);
            foreach (var q in examEntity.Questions)
            {
                q.IsActive = false;
                foreach (var a in q.Answers)
                    a.IsActive = false;
            }
            foreach (var q in examEntity.Questions)
            {
                var category = await categoryRepo.SingleOrDefaultAsync(
                        predicate: c =>
                            c.Name.Trim().ToLower() == request.CategoryName.Trim().ToLower() &&
                            c.Grade.Trim().ToLower() == request.CategoryGrade.Trim().ToLower()
                    );

                if (category == null)
                {   
                    category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = request.CategoryName,
                        Grade = request.CategoryGrade,
                        IsActive = true,
                        CreateAt = DateTime.UtcNow
                    };
                    await categoryRepo.InsertAsync(category);
                }

                q.CategoryId = category.Id;
            }


            await examRepo.InsertAsync(examEntity);
            await _unitOfWork.CommitAsync();

         
            // 6. Map response
            var response = _mapper.Map<ExamExchangeResponse>(examEntity);
            foreach (var q in response.Questions)
            {
                q.CategoryName = request.CategoryName;
                q.CategoryGrade = request.CategoryGrade;
            }
            // 7. Trả về theo mẫu của bạn
            return new BaseResponse<ExamExchangeResponse>
            {
                Status = StatusCodes.Status201Created.ToString(),
                Message = "Tạo đề thi thành công",
                Data = response
            };

        }



        Task<BaseResponse<ExamExchangeResponse?>> IExamExchangeService.GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<BaseResponse<IEnumerable<ExamExchangeResponse>>> IExamExchangeService.GetByTeacherAsync(Guid teacherId, string? status)
        {
            throw new NotImplementedException();
        }
    }
}
