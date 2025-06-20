using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamMatrix;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamMatrix;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class ExamMatrixService : BaseService<ExamMatrixService>, IExamMatrixService
    {
        public ExamMatrixService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<ExamMatrixService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor): base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<GetExamMatrixResponse>> CreateExamMatrix(CreateExamMatrixWithStructureRequest request)
        {
            if (request == null || request.Sections == null || !request.Sections.Any())
            {
                return new BaseResponse<GetExamMatrixResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Dữ liệu tạo ma trận không hợp lệ hoặc không có section nào."
                };
            }

            foreach (var sectionReq in request.Sections)
            {
                if (sectionReq.TotalQuestions <= 0 || sectionReq.TotalScore <= 0)
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Section '{sectionReq.SectionName}' có tổng số câu hoặc tổng điểm không hợp lệ."
                    };
                }

                if (sectionReq.Details == null || !sectionReq.Details.Any())
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Section '{sectionReq.SectionName}' không có chi tiết section."
                    };
                }

                int totalQuestionCount = sectionReq.Details.Sum(d => d.QuestionCount);
                double totalScore = sectionReq.Details.Sum(d => d.QuestionCount * d.ScorePerQuestion);

                if (totalQuestionCount > sectionReq.TotalQuestions)
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Tổng số câu của các detail vượt quá TotalQuestions trong section '{sectionReq.SectionName}'."
                    };
                }

                if (totalScore > sectionReq.TotalScore)
                {
                    return new BaseResponse<GetExamMatrixResponse>
                    {
                        Status = StatusCodes.Status400BadRequest.ToString(),
                        Message = $"Tổng điểm các detail vượt quá TotalScore trong section '{sectionReq.SectionName}'."
                    };
                }

                foreach (var detail in sectionReq.Details)
                {
                    if (detail.QuestionCount <= 0 || detail.ScorePerQuestion <= 0)
                    {
                        return new BaseResponse<GetExamMatrixResponse>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Chi tiết trong section '{sectionReq.SectionName}' có giá trị QuestionCount hoặc ScorePerQuestion không hợp lệ."
                        };
                    }
                }
            }

            var matrix = _mapper.Map<ExamMatrix>(request);
            await _unitOfWork.GetRepository<ExamMatrix>().InsertAsync(matrix);

            foreach (var sectionReq in request.Sections)
            {
                var section = _mapper.Map<MatrixSection>(sectionReq);
                section.ExamMatrixId = matrix.Id;
                await _unitOfWork.GetRepository<MatrixSection>().InsertAsync(section);

                foreach (var detailReq in sectionReq.Details)
                {
                    var detail = _mapper.Map<MatrixSectionDetail>(detailReq);
                    detail.MatrixSectionId = section.Id;
                    await _unitOfWork.GetRepository<MatrixSectionDetail>().InsertAsync(detail);
                }
            }

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<GetExamMatrixResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình tạo ma trận",
                };
            }

            return new BaseResponse<GetExamMatrixResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo ma trận thành công.",
                Data = _mapper.Map<GetExamMatrixResponse>(matrix)
            };
        }

        public Task<BaseResponse<bool>> DeleteExamMatrix(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IPaginate<GetExamMatrixResponse>>> GetAllExamMatrix(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetExamMatrixResponse>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ExamMatrixStructureResponse>> GetMatrixStructure(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> UpdateExamMatrix(Guid id, UpdateExamMatrixRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
    