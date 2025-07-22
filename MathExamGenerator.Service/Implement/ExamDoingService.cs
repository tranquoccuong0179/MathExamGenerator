using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamDoing;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Payload.Response.ExamDoing;
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
using Microsoft.Identity.Client;

namespace MathExamGenerator.Service.Implement
{
    public class ExamDoingService : BaseService<ExamDoingService>, IExamDoingService
    {
        public ExamDoingService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<ExamDoingService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateExamDoingResponse>> Create(CreateExamDoingRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse<CreateExamDoingResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                    Data = null
                };
            }

            if (request.ExamId == null)
            {
                return new BaseResponse<CreateExamDoingResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Phải cung cấp ExamId.",
                    Data = null
                };
            }

            var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                predicate: a => a.AccountId.Equals(accountId) && a.IsActive == true);

            if (wallet == null) 
            {
                return new BaseResponse<CreateExamDoingResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy ví của tài khoản.",
                    Data = null
                };
            }

            if (account.FreeTries <= 0 && wallet.Point <= 5000)
            {
                return new BaseResponse<CreateExamDoingResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Bạn không còn đủ số lần free và point để mua Exam này.",
                    Data = null
                };
            }

            var examDoing = _mapper.Map<ExamDoing>(request);
            examDoing.AccountId = accountId;
            examDoing.Grade = 0;
            examDoing.Status = ExamDoingEnum.Processing.ToString();

            if (account.FreeTries > 0)
            {
                account.FreeTries--;
                account.UpdateAt = TimeUtil.GetCurrentSEATime();
                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            } 
            else
            {
                wallet.Point -= 5000;
                wallet.UpdateAt = TimeUtil.GetCurrentSEATime();
                _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    WalletId = wallet.Id,
                    DepositId = null,
                    PackageOrderId = null,
                    ExamDoingId = examDoing.Id,
                    Type = "Thanh toán",
                    Description = "Mua đề thi",
                    Status = "Success",
                    Amount = 5000.00m,
                    IsActive = true,
                    CreateAt = TimeUtil.GetCurrentSEATime(),
                };

                await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
            }

            List<Guid> questionIds = new();

            var exam = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.Id == request.ExamId);

            if (exam == null)
            {
                return new BaseResponse<CreateExamDoingResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy Exam.",
                    Data = null
                };
            }

            var examQuestions = await _unitOfWork.GetRepository<ExamQuestion>().GetListAsync(
                predicate: x => x.ExamId == request.ExamId);

            if (!examQuestions.Any())
            {
                return new BaseResponse<CreateExamDoingResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy câu hỏi cho bài thi.",
                    Data = null
                };
            }

            questionIds = examQuestions
                .Where(x => x.QuestionId.HasValue)
                .Select(x => x.QuestionId.Value)
                .ToList();           

            var questions = await _unitOfWork.GetRepository<Question>().GetListAsync(
                predicate: q => questionIds.Contains(q.Id),
                include: q => q.Include(q => q.Answers));

            var questionHistories = questions.Select(q =>
            {
                var correctAnswer = q.Answers.FirstOrDefault(a => a.IsTrue == true)?.Content;

                return new QuestionHistory
                {
                    Id = Guid.NewGuid(),
                    ExamDoingId = examDoing.Id,
                    QuestionId = q.Id,
                    Answer = correctAnswer,
                    YourAnswer = null,
                    IsActive = true,
                    CreateAt = TimeUtil.GetCurrentSEATime(),
                    UpdateAt = TimeUtil.GetCurrentSEATime()
                };
            }).ToList();

            examDoing.QuestionHistories = questionHistories;

            await _unitOfWork.GetRepository<ExamDoing>().InsertAsync(examDoing);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<CreateExamDoingResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình tạo lịch sử đề thi.",
                };
            }

            return new BaseResponse<CreateExamDoingResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo lịch sử thi thành công.",
                Data = _mapper.Map<CreateExamDoingResponse>(examDoing)
            };
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            var examDoing = await _unitOfWork.GetRepository<ExamDoing>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: q => q.Include(x => x.QuestionHistories));

            if (examDoing == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử thi.",
                    Data = false
                };
            }

            examDoing.IsActive = false;
            examDoing.DeleteAt = TimeUtil.GetCurrentSEATime();
            examDoing.UpdateAt = TimeUtil.GetCurrentSEATime();

            foreach (var qh in examDoing.QuestionHistories)
            {
                qh.IsActive = false;
                qh.DeleteAt = TimeUtil.GetCurrentSEATime();
                qh.UpdateAt = TimeUtil.GetCurrentSEATime();

                _unitOfWork.GetRepository<QuestionHistory>().UpdateAsync(qh);
            }

            _unitOfWork.GetRepository<ExamDoing>().UpdateAsync(examDoing);

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

        public async Task<BaseResponse<IPaginate<ExamDoingOverviewResponse>>> GetAll(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<ExamDoingOverviewResponse>>
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
                return new BaseResponse<IPaginate<ExamDoingOverviewResponse>>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                    Data = null
                };
            }

            var result = await _unitOfWork.GetRepository<ExamDoing>().GetPagingListAsync(
                selector: x => _mapper.Map<ExamDoingOverviewResponse>(x),
                page: page,
                size: size,
                orderBy: q => q.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true && x.AccountId == account.Id,
                include: q => q.Include(x => x.Exam));

            if (result?.Items != null && result.Items.Any())
            {
                var examDoingIds = result.Items.Select(x => x.Id).ToList();

                var questionHistories = await _unitOfWork.GetRepository<QuestionHistory>()
                    .GetListAsync(predicate: q => examDoingIds.Contains(q.ExamDoingId.Value));

                foreach (var item in result.Items)
                {
                    var questions = questionHistories.Where(x => x.ExamDoingId == item.Id);

                    int total = questions.Count();

                    int done = questions.Count(x => x.YourAnswer != null);

                    item.TotalQuestion = $"{done}/{total}";
                }
            }


            return new BaseResponse<IPaginate<ExamDoingOverviewResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách lịch sử thi thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<GetExamDoingResponse>> GetById(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<ExamDoing>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: q => q
                    .Include(x => x.QuestionHistories)
                    .Include(x => x.Exam));

            if (entity == null)
            {
                return new BaseResponse<GetExamDoingResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử thi.",
                    Data = null
                };
            }

            int total = entity.QuestionHistories.Count();

            int done = entity.QuestionHistories.Count(x => x.YourAnswer != null);

            var response = _mapper.Map<GetExamDoingResponse>(entity);
            response.Name = entity.Exam?.Name;
            response.TotalQuestion = $"{done}/{total}";

            return new BaseResponse<GetExamDoingResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy lịch sử thi thành công.",
                Data = response
            };
        }

        public async Task<BaseResponse<List<GetQuestionHistoryResponse>>> GetQuestionHistoriesByTestId(Guid id)
        {
            var examDoing = await _unitOfWork.GetRepository<ExamDoing>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (examDoing == null)
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
                predicate: x => x.ExamDoingId == id && x.IsActive == true
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

        public async Task<BaseResponse<bool>> Update(Guid id, UpdateExamDoingRequest request, ExamDoingEnum? status)
        {
            var examDoing = await _unitOfWork.GetRepository<ExamDoing>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: x => x.Include(th => th.QuestionHistories));

            if (examDoing == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử thi.",
                    Data = false
                };
            }

            examDoing.ExamId = request.ExamId ?? examDoing.ExamId;
            if (status.HasValue)
            {
                examDoing.Status = status.ToString();
            }
            examDoing.Duration = request.Duration ?? examDoing.Duration;
            examDoing.UpdateAt = TimeUtil.GetCurrentSEATime();

            if (request.QuestionHistories != null && request.QuestionHistories.Any())
            {
                var updateMap = request.QuestionHistories.ToDictionary(x => x.Id, x => x.YourAnswer);

                foreach (var qh in examDoing.QuestionHistories)
                {
                    if (updateMap.TryGetValue(qh.Id, out var yourAnswer))
                    {
                        qh.YourAnswer = yourAnswer;
                        qh.UpdateAt = TimeUtil.GetCurrentSEATime();
                        _unitOfWork.GetRepository<QuestionHistory>().UpdateAsync(qh);
                    }
                }
            }

            if (examDoing.Status == ExamDoingEnum.Finish.ToString())
            {
                examDoing.Grade = 10.0/examDoing.QuestionHistories.Count()*examDoing.QuestionHistories.Count(x => x.Answer==x.YourAnswer);

                if (examDoing.Grade >= 9.0)
                {
                    var wallet = await _unitOfWork.GetRepository<Wallet>().SingleOrDefaultAsync(
                        predicate: a => a.AccountId.Equals(examDoing.AccountId) && a.IsActive == true);

                    if (wallet == null)
                    {
                        return new BaseResponse<bool>
                        {
                            Status = StatusCodes.Status404NotFound.ToString(),
                            Message = "Không tìm thấy ví của tài khoản.",
                            Data = false
                        };
                    }

                    wallet.Point += 2000;
                    wallet.UpdateAt = TimeUtil.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);

                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = wallet.Id,
                        DepositId = null,
                        PackageOrderId = null,
                        ExamDoingId = examDoing.Id,
                        Type = "Điểm thưởng",
                        Description = "Thưởng bài thi đạt điểm cao",
                        Status = "Success",
                        Amount = 2000.00m,
                        IsActive = true,
                        CreateAt = TimeUtil.GetCurrentSEATime(),
                    };

                    await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
                }
            }

            _unitOfWork.GetRepository<ExamDoing>().UpdateAsync(examDoing);

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

            if (examDoing.Status == ExamDoingEnum.Finish.ToString() && examDoing.Grade >= 9.0)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status200OK.ToString(),
                    Message = "Người dùng đã hoàn thành bài thi và nhận được điểm thưởng với điểm thi >= 9.0",
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
