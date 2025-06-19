using AutoMapper;
using Azure;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Exam;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Model.Payload.Response.Question;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MathExamGenerator.Service.Implement
{
    public class ExamService : BaseService<ExamService>, IExamService
    {
        public ExamService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<ExamService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateExamResponse>> CreateExam(CreateExamRequest request)
        {
            if (request == null || request.ExamMatrixId == null)
            {
                return new BaseResponse<CreateExamResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Yêu cầu không hợp lệ."
                };
            }

            var examMatrix = await _unitOfWork.GetRepository<ExamMatrix>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(request.ExamMatrixId) && s.IsActive == true);
            if (examMatrix == null)
            {
                throw new NotFoundException("Không tìm thấy ma trận đề thi.");
            }

            var exam = _mapper.Map<Exam>(request);
            await _unitOfWork.GetRepository<Exam>().InsertAsync(exam);

            var sections = await _unitOfWork.GetRepository<MatrixSection>().GetListAsync(
                predicate: s => s.ExamMatrixId == examMatrix.Id && s.IsActive == true);
            if (!sections.Any())
            {
                throw new NotFoundException("Không có section nào trong ma trận đề.");
            }

            var examQuestionRepo = _unitOfWork.GetRepository<ExamQuestion>();
            var questionRepo = _unitOfWork.GetRepository<Question>();

            foreach (var section in sections)
            {
                var details = await _unitOfWork.GetRepository<MatrixSectionDetail>().GetListAsync(
                    predicate: d => d.MatrixSectionId == section.Id && d.IsActive == true);
            
                foreach (var detail in details)
                {
                    var questionsQuery = await questionRepo.GetListAsync(
                        predicate: q => q.IsActive == true && 
                                        q.Level == detail.Difficulty && 
                                        q.BookTopicId == detail.BookTopicId && 
                                        q.BookTopic.BookChapterId == detail.BookChapterId);

                    if (questionsQuery.Count < detail.QuestionCount)
                    {
                        throw new Exception($"Không đủ câu hỏi cho section {section.SectionName} – độ khó {detail.Difficulty}.");
                    }

                    var selectedQuestions = questionsQuery
                        .OrderBy(q => Guid.NewGuid()) 
                        .Take(detail.QuestionCount.Value)
                        .ToList();

                    foreach (var q in selectedQuestions)
                    {
                        await examQuestionRepo.InsertAsync(new ExamQuestion
                        {
                            Id = Guid.NewGuid(),
                            ExamId = exam.Id,
                            QuestionId = q.Id,
                            CreateAt = TimeUtil.GetCurrentSEATime(),
                            UpdateAt = TimeUtil.GetCurrentSEATime(),
                            IsActive = true
                        });
                    }
                }
            }

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo đề thi.");
            }

            return new BaseResponse<CreateExamResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo đề thi thành công",
                Data = _mapper.Map<CreateExamResponse>(exam)
            };
        }

        public Task<BaseResponse<bool>> DeleteExam(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IPaginate<GetExamResponse>>> GetAllExam(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<List<GetQuestionResponse>>> GetAllQuestionByExam(Guid examId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<GetExamResponse>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> UpdateExam(Guid id, UpdateExamRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
