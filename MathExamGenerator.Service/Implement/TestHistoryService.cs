using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.TestHistory;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Payload.Response.TestHistory;
using MathExamGenerator.Model.Payload.Response.TestStorage;
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

namespace MathExamGenerator.Service.Implement
{
    public class TestHistoryService : BaseService<TestHistoryService>, ITestHistoryService
    {
        public TestHistoryService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<TestHistoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateTestHistoryResponse>> Create(CreateTestHistoryRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse<CreateTestHistoryResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                    Data = null
                };
            }

            if (request.ExamId == null && request.QuizId == null)
            {
                return new BaseResponse<CreateTestHistoryResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Phải cung cấp ExamId hoặc QuizId.",
                    Data = null
                };
            }

            if (request.ExamId != null && request.QuizId != null)
            {
                return new BaseResponse<CreateTestHistoryResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Chỉ được chọn ExamId hoặc QuizId, không được cả hai.",
                    Data = null
                };
            }

            var testHistory = _mapper.Map<TestHistory>(request);
            testHistory.AccountId = accountId;
            testHistory.Grade = 0;

            List<Guid> questionIds = new();

            if (request.ExamId.HasValue)
            {
                var exam = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                    predicate: x => x.Id == request.ExamId);

                if (exam == null)
                {
                    return new BaseResponse<CreateTestHistoryResponse>
                    {
                        Status = StatusCodes.Status404NotFound.ToString(),
                        Message = "Không tìm thấy Exam.",
                        Data = null
                    };
                }

                var examQuestions = await _unitOfWork.GetRepository<ExamQuestion>().GetListAsync(
                    predicate: x => x.ExamId == request.ExamId);

                questionIds = examQuestions
                    .Where(x => x.QuestionId.HasValue)
                    .Select(x => x.QuestionId.Value)
                    .ToList();
            }
            else if (request.QuizId.HasValue)
            {
                var quiz = await _unitOfWork.GetRepository<Quiz>().SingleOrDefaultAsync(
                    predicate: x => x.Id == request.QuizId);

                if (quiz == null)
                {
                    return new BaseResponse<CreateTestHistoryResponse>
                    {
                        Status = StatusCodes.Status404NotFound.ToString(),
                        Message = "Không tìm thấy Quiz.",
                        Data = null
                    };
                }

                var quizQuestions = await _unitOfWork.GetRepository<QuizQuestion>().GetListAsync(
                    predicate: x => x.QuizId == request.QuizId);

                questionIds = quizQuestions
                    .Where(x => x.QuestionId.HasValue)
                    .Select(x => x.QuestionId.Value)
                    .ToList();
            }

            var questions = await _unitOfWork.GetRepository<Question>().GetListAsync(
                predicate: q => questionIds.Contains(q.Id),
                include: q => q.Include(q => q.Answers));

            var questionHistories = questions.Select(q =>
            {
                var correctAnswer = q.Answers.FirstOrDefault(a => a.IsTrue == true)?.Content;

                return new QuestionHistory
                {
                    Id = Guid.NewGuid(),
                    HistoryTestId = testHistory.Id,
                    QuestionId = q.Id,
                    Answer = correctAnswer,
                    YourAnswer = null,
                    IsActive = true,
                    CreateAt = TimeUtil.GetCurrentSEATime(),
                    UpdateAt = TimeUtil.GetCurrentSEATime()
                };
            }).ToList();

            testHistory.QuestionHistories = questionHistories;

            await _unitOfWork.GetRepository<TestHistory>().InsertAsync(testHistory);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<CreateTestHistoryResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình tạo lịch sử đề thi.",
                };
            }

            return new BaseResponse<CreateTestHistoryResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo lịch sử thi thành công.",
                Data = _mapper.Map<CreateTestHistoryResponse>(testHistory)
            };
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            var testHistory = await _unitOfWork.GetRepository<TestHistory>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: q => q.Include(x => x.QuestionHistories));

            if (testHistory == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử thi.",
                    Data = false
                };
            }

            testHistory.IsActive = false;
            testHistory.DeleteAt = TimeUtil.GetCurrentSEATime();
            testHistory.UpdateAt = TimeUtil.GetCurrentSEATime();

            foreach (var qh in testHistory.QuestionHistories)
            {
                qh.IsActive = false;
                qh.DeleteAt = TimeUtil.GetCurrentSEATime();
                qh.UpdateAt = TimeUtil.GetCurrentSEATime();

                _unitOfWork.GetRepository<QuestionHistory>().UpdateAsync(qh);
            }

            _unitOfWork.GetRepository<TestHistory>().UpdateAsync(testHistory);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình xóa lịch sử đề thi.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xoá lịch sử thi thành công.",
                Data = isSuccessfully
            };
        }

        public async Task<BaseResponse<IPaginate<TestHistoryOverviewResponse>>> GetAll(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<TestHistoryOverviewResponse>>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Page và Size phải lớn hơn 0.",
                    Data = null
                };
            }

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse<IPaginate<TestHistoryOverviewResponse>>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                    Data = null
                };
            }

            var result = await _unitOfWork.GetRepository<TestHistory>().GetPagingListAsync(
                selector: x => _mapper.Map<TestHistoryOverviewResponse>(x),
                page: page,
                size: size,
                orderBy: q => q.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true && x.AccountId == account.Id,
                include: q => q.Include(x => x.Exam).Include(x => x.Quiz));

            return new BaseResponse<IPaginate<TestHistoryOverviewResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách lịch sử thi thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<GetTestHistoryResponse>> GetById(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<TestHistory>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: q => q
                    .Include(x => x.QuestionHistories)
                    .Include(x => x.Exam)
                    .Include(x => x.Quiz));

            if (entity == null)
            {
                return new BaseResponse<GetTestHistoryResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử thi.",
                    Data = null
                };
            }

            var response = _mapper.Map<GetTestHistoryResponse>(entity);
            response.Name = entity.Exam?.Name ?? entity.Quiz?.Name;

            return new BaseResponse<GetTestHistoryResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy lịch sử thi thành công.",
                Data = response
            };
        }

        public async Task<BaseResponse<List<GetQuestionHistoryResponse>>> GetQuestionHistoriesByTestId(Guid id)
        {
            var testHistory = await _unitOfWork.GetRepository<TestHistory>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (testHistory == null)
            {
                return new BaseResponse<List<GetQuestionHistoryResponse>>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử thi.",
                    Data = null
                };
            }

            // Lấy danh sách QuestionHistory theo TestHistoryId
            var questionHistories = await _unitOfWork.GetRepository<QuestionHistory>().GetListAsync(
                predicate: x => x.HistoryTestId == id && x.IsActive == true
            );

            var result = questionHistories
                .Select(qh => _mapper.Map<GetQuestionHistoryResponse>(qh))
                .ToList();

            return new BaseResponse<List<GetQuestionHistoryResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách câu hỏi lịch sử thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<bool>> Update(Guid id, UpdateTestHistoryRequest request)
        {
            var testHistory = await _unitOfWork.GetRepository<TestHistory>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: x => x.Include(th => th.QuestionHistories));

            if (testHistory == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử thi.",
                    Data = false
                };
            }

            testHistory.ExamId = request.ExamId ?? testHistory.ExamId;
            testHistory.QuizId = request.QuizId ?? testHistory.QuizId;
            testHistory.Grade = request.Grade ?? testHistory.Grade;
            testHistory.Status = request.Status ?? testHistory.Status;
            testHistory.StartAt = request.StartAt ?? testHistory.StartAt;
            testHistory.UpdateAt = TimeUtil.GetCurrentSEATime();

            if (request.QuestionHistories != null && request.QuestionHistories.Any())
            {
                var updateMap = request.QuestionHistories.ToDictionary(x => x.Id, x => x.YourAnswer);

                foreach (var qh in testHistory.QuestionHistories)
                {
                    if (updateMap.TryGetValue(qh.Id, out var yourAnswer))
                    {
                        qh.YourAnswer = yourAnswer;
                        qh.UpdateAt = TimeUtil.GetCurrentSEATime();
                        _unitOfWork.GetRepository<QuestionHistory>().UpdateAsync(qh);
                    }
                }
            }

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình cập nhật lịch sử đề thi.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật lịch sử thi thành công.",
                Data = isSuccessfully
            };
        }
    }
}
