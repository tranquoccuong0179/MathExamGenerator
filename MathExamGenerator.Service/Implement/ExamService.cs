using AutoMapper;
using Azure;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Exam;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse<CreateExamResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                };
            }

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
                return new BaseResponse<CreateExamResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ma trận đề thi.",
                };
            }

            var exam = _mapper.Map<Exam>(request);
            exam.AccountId = account.Id;
            exam.Status = ExamEnum.Pending.ToString();
            await _unitOfWork.GetRepository<Exam>().InsertAsync(exam);

            var sections = await _unitOfWork.GetRepository<MatrixSection>().GetListAsync(
                predicate: s => s.ExamMatrixId == examMatrix.Id && s.IsActive == true);
            if (!sections.Any())
            {
                return new BaseResponse<CreateExamResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Không có section nào trong ma trận đề.",
                };
            }

            var examQuestionRepo = _unitOfWork.GetRepository<ExamQuestion>();
            var questionRepo = _unitOfWork.GetRepository<Question>();

            HashSet<Guid> selectedQuestionIds = new();

            foreach (var section in sections)
            {
                var details = await _unitOfWork.GetRepository<MatrixSectionDetail>().GetListAsync(
                    predicate: d => d.MatrixSectionId == section.Id && d.IsActive == true);
            
                foreach (var detail in details)
                {
                    IEnumerable<Question> questionsQuery = new List<Question>();

                    if (detail.BookTopicId.HasValue)
                    {
                        questionsQuery = await questionRepo.GetListAsync(
                            predicate: q => q.IsActive == true &&
                                            q.Level == detail.Difficulty &&
                                            q.BookTopicId == detail.BookTopicId &&
                                            !selectedQuestionIds.Contains(q.Id));
                    }
                    else if (detail.BookChapterId.HasValue)
                    {
                        questionsQuery = await questionRepo.GetListAsync(
                            predicate: q => q.IsActive == true &&
                                            q.Level == detail.Difficulty &&
                                            q.BookTopic != null &&
                                            q.BookTopic.BookChapterId == detail.BookChapterId &&
                                            !selectedQuestionIds.Contains(q.Id));
                    }
                    else
                    {
                        return new BaseResponse<CreateExamResponse>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Thiếu thông tin BookTopicId hoặc BookChapterId ở section {section.SectionName}",
                        };
                    }

                    if (questionsQuery.Count() < detail.QuestionCount)
                    {
                        return new BaseResponse<CreateExamResponse>
                        {
                            Status = StatusCodes.Status400BadRequest.ToString(),
                            Message = $"Không đủ câu hỏi cho section {section.SectionName} – độ khó {detail.Difficulty}.",
                        };
                    }

                    var random = new Random();
                    var selectedQuestions = questionsQuery
                        .OrderBy(r => random.Next())
                        .Take(detail.QuestionCount.Value)
                        .ToList();

                    foreach (var q in selectedQuestions)
                    {
                        selectedQuestionIds.Add(q.Id);

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
                return new BaseResponse<CreateExamResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình tạo đề thi.",
                };
            }

            return new BaseResponse<CreateExamResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo đề thi thành công",
                Data = _mapper.Map<CreateExamResponse>(exam)
            };
        }

        public async Task<BaseResponse<bool>> DeleteExam(Guid id)
        {
            var exam = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (exam == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy đề thi để xóa.",
                    Data = false
                };
            }

            exam.IsActive = false;
            exam.UpdateAt = TimeUtil.GetCurrentSEATime();
            exam.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Exam>().UpdateAsync(exam);

            var examQuestions = await _unitOfWork.GetRepository<ExamQuestion>().GetListAsync(
                predicate: x => x.ExamId == id && x.IsActive == true);

            foreach (var eq in examQuestions)
            {
                eq.IsActive = false;
                eq.UpdateAt = TimeUtil.GetCurrentSEATime();
                eq.DeleteAt = TimeUtil.GetCurrentSEATime();
                _unitOfWork.GetRepository<ExamQuestion>().UpdateAsync(eq);
            }

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Xoá đề thi thất bại.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xoá đề thi thành công.",
                Data = isSuccessfully
            };
        }

        public async Task<BaseResponse<IPaginate<GetExamResponse>>> GetAllExam(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<GetExamResponse>>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Trang hoặc kích thước không hợp lệ.",
                    Data = null
                };
            }

            var exams = await _unitOfWork.GetRepository<Exam>().GetPagingListAsync(
                selector: x => _mapper.Map<GetExamResponse>(x),
                page: page,
                size: size,
                orderBy: o => o.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true,
                include: x => x.Include(e => e.ExamMatrix));

            return new BaseResponse<IPaginate<GetExamResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách đề thi thành công.",
                Data = exams
            };
        }

        public async Task<BaseResponse<ExamWithQuestionsResponse>> GetAllQuestionByExam(Guid examId)
        {
            var exam = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.Id == examId && x.IsActive == true);

            if (exam == null)
            {
                return new BaseResponse<ExamWithQuestionsResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy đề thi."
                };
            }

            var examQuestions = await _unitOfWork.GetRepository<ExamQuestion>().GetListAsync(
                predicate: eq => eq.ExamId == examId && eq.IsActive == true,
                include: eq => eq.Include(e => e.Question)
                                 .ThenInclude(q => q.Answers));

            var questions = examQuestions
                .Where(eq => eq.Question != null)
                .Select(eq => _mapper.Map<QuestionWithAnswerResponse>(eq.Question))
                .ToList();

            var response = _mapper.Map<ExamWithQuestionsResponse>(exam);
            response.Questions = questions;

            return new BaseResponse<ExamWithQuestionsResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy đề thi và danh sách câu hỏi thành công.",
                Data = response
            };
        }

        public async Task<BaseResponse<GetExamResponse>> GetById(Guid id)
        {
            var exam = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: x => x.Include(e => e.ExamMatrix));

            if (exam == null)
            {
                return new BaseResponse<GetExamResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy đề thi."
                };
            }

            return new BaseResponse<GetExamResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy đề thi thành công.",
                Data = _mapper.Map<GetExamResponse>(exam)
            };
        }

        public async Task<BaseResponse<IPaginate<GetExamResponse>>> GetExamsOfCurrentUser(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<GetExamResponse>>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Trang hoặc kích thước không hợp lệ.",
                    Data = null
                };
            }

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse<IPaginate<GetExamResponse>>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                };
            }

            var exams = await _unitOfWork.GetRepository<Exam>().GetPagingListAsync(
                selector: x => _mapper.Map<GetExamResponse>(x),
                page: page,
                size: size,
                orderBy: o => o.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true && x.AccountId == account.Id,
                include: x => x.Include(e => e.ExamMatrix));

            return new BaseResponse<IPaginate<GetExamResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách đề thi thành công.",
                Data = exams
            };
        }

        public async Task<BaseResponse<bool>> UpdateExam(Guid id, UpdateExamRequest request, ExamEnum? examEnum)
        {
            var exam = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (exam == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy đề thi để cập nhật.",
                    Data = false
                };
            }

            exam.Name = request.Name ?? exam.Name;
            exam.Time = request.Time ?? exam.Time;
            if (examEnum.HasValue)
            {
                exam.Status = examEnum.ToString();
            }
            exam.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Exam>().UpdateAsync(exam);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Cập nhật thất bại.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật đề thi thành công.",
                Data = isSuccessfully
            };
        }
    }
}
