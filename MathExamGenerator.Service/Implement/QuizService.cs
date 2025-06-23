using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Quiz;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamExchange;
using MathExamGenerator.Model.Payload.Response.Question;
using MathExamGenerator.Model.Payload.Response.Quiz;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class QuizService : BaseService<QuizService>, IQuizService
    {
        public QuizService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<QuizService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateQuizResponse>> CreateQuiz(CreateQuizRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true) ?? throw new NotFoundException("Không tìm thấy tài khoản");

            if (account.QuizFree <= 0)
            {
                throw new BadHttpRequestException("Bạn không lượt free để tạo quiz");
            }

            if (request.BookChapterId.HasValue && request.BookTopicId.HasValue)
            {
                throw new BadHttpRequestException("Chỉ chọn 1 trong 2 là topic hoặc chapter");
            }

            if (request.Quantity <= 0)
            {
                throw new BadHttpRequestException("Số lượng câu hỏi phải lớn hơn 0");
            }

            if (request.Time <= 0)
            {
                throw new BadHttpRequestException("Thời gian làm bài phải lớn hơn 0 phút");
            }

            ICollection<Question> questions; 

            if (request.BookTopicId.HasValue)
            {
                var bookTopic = await _unitOfWork.GetRepository<BookTopic>().SingleOrDefaultAsync(
                    predicate: b => b.Id.Equals(request.BookTopicId) && b.IsActive == true) ?? throw new NotFoundException("Không tìm thấy chủ đề muốn tạo");

                questions = await _unitOfWork.GetRepository<Question>().GetListAsync(
                    predicate: q => q.BookTopicId == request.BookTopicId && q.IsActive == true);
            }
            else if (request.BookChapterId.HasValue)
            {
                var bookChapter = await _unitOfWork.GetRepository<BookChapter>().SingleOrDefaultAsync(
                    predicate: b => b.Id.Equals(request.BookChapterId) && b.IsActive == true,
                    include: b => b.Include(b => b.BookTopics)) ?? throw new NotFoundException("Không tìm thấy chương muốn tạo");

                var topicIds = bookChapter.BookTopics
                    .Where(bt => bt.IsActive == true)
                    .Select(bt => bt.Id)
                    .ToList();

                questions = await _unitOfWork.GetRepository<Question>().GetListAsync(
                    predicate: q => q.BookTopicId.HasValue && topicIds.Contains(q.BookTopicId.Value) && q.IsActive == true
                );

            }
            else
            {
                throw new BadHttpRequestException("Phải truyền topic hoặc chapter");
            }

            if (questions.Count() < request.Quantity)
            {
                throw new BadHttpRequestException("Số câu trong hệ thống không đủ để tạo ra quiz cho bạn");
            }

            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Quantity = request.Quantity,
                Time = request.Time,
                BookTopicId = request.BookTopicId,
                BookChapterId = request.BookChapterId,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Quiz>().InsertAsync(quiz);

            var random = new Random();
            var randomQuestions = questions.OrderBy(q => random.Next())
                                     .Take(request.Quantity)
                                     .ToList();

            var quizQuestions = randomQuestions.Select(q => new QuizQuestion
            {
                Id = Guid.NewGuid(),
                QuizId = quiz.Id,
                QuestionId = q.Id,
                IsActive = true,
                CreateAt = TimeUtil.GetCurrentSEATime()
            }).ToList();

            await _unitOfWork.GetRepository<QuizQuestion>().InsertRangeAsync(quizQuestions);

            account.QuizFree -= 1;
            account.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<Account>().UpdateAsync(account);

            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccess)
            {
                throw new Exception("Một lỗi đã xảy ra trong quá trình tạo quiz");
            }

            return new BaseResponse<CreateQuizResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo quiz thành công",
                Data = new CreateQuizResponse
                {
                    Name = quiz.Name,
                    Time = quiz.Time,
                    Quantity = quiz.Quantity,
                    CreateAt = quiz.CreateAt,
                }
            };
        }

        public async Task<BaseResponse<IPaginate<GetQuizResponse>>> GetAllQuiz(int page, int size)
        {
            var quizzes = await _unitOfWork.GetRepository<Quiz>().GetPagingListAsync(
                selector: q => new GetQuizResponse
                {
                    Id = q.Id,
                    Name = q.Name,
                    Quantity = q.Quantity,
                    Time = q.Time,
                    CreateAt = q.CreateAt
                },
                predicate: q => q.IsActive == true,
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetQuizResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách quiz thành công",
                Data = quizzes
            };
        }

        public async Task<BaseResponse<GetQuizDetailResponse>> GetQuizDetail(Guid id)
        {
            var quiz = await _unitOfWork.GetRepository<Quiz>().SingleOrDefaultAsync(
                selector: q => new GetQuizDetailResponse
                {
                    Id = q.Id,
                    Name = q.Name,
                    Quantity = q.Quantity,
                    Time = q.Time,
                    CreateAt = q.CreateAt,
                    Questions = q.QuizQuestions
                    .Where(qq => qq.IsActive == true && qq.Question != null)
                    .Select(qq => new QuestionResponse
                        {
                            BookTopicId = qq.Question.BookTopicId,
                            Level = qq.Question.Level,
                            Content = qq.Question.Content,
                            Solution = qq.Question.Solution,
                            Image = qq.Question.Image,
                            CategoryId = qq.Question.CategoryId.Value,
                            CategoryName = qq.Question.Category.Name,
                            CategoryGrade = qq.Question.Category.Grade,
                            Answers = qq.Question.Answers.Select(a => new AnswerResponse
                            {
                                Id = a.Id,
                                Content = a.Content,
                                Image = a.Image,
                                IsTrue = a.IsTrue.Value
                            }).ToList(),
                    }).ToList()
                },
                predicate: q => q.Id.Equals(id) && q.IsActive == true,
                include: q => q.Include(q => q.QuizQuestions)
                               .ThenInclude(qq => qq.Question)
                               .ThenInclude(q => q.Category)
                               .Include(q => q.QuizQuestions)
                               .ThenInclude(qq => qq.Question)
                               .ThenInclude(q => q.Answers)) ?? throw new NotFoundException("Không tìm thấy bài quiz");

            return new BaseResponse<GetQuizDetailResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thông tin bài quiz thành công",
                Data = quiz
            };
        }
    }
}
