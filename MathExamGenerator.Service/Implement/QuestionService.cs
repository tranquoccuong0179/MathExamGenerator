﻿using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Answer;
using MathExamGenerator.Model.Payload.Response.Question;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class QuestionService : BaseService<QuestionService>, IQuestionService
    {
        public QuestionService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<QuestionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<IPaginate<QuestionResponse>>> GetAllQuestion(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<Question>().GetPagingListAsync(
                 selector: q => new QuestionResponse
                 {
                     Id = q.Id,
                     BookTopicId = q.BookTopicId,
                     Level = q.Level,
                     Content = q.Content,
                     Solution = q.Solution,
                     Image = q.Image,
                     CategoryId = q.CategoryId.Value,
                     CategoryName = q.Category.Name.ToString(),
                     CategoryGrade = q.Category.Grade.ToString(),
                     Answers = q.Answers.Select(a => new AnswerResponse
                     {
                         Id = a.Id,
                         Content = a.Content,
                         IsTrue = a.IsTrue.Value,
                     }).ToList()
                 },
                   predicate: q => q.IsActive == true && q.DeleteAt == null,
                   include: q => q.Include(x => x.Category).Include(x => x.Answers),
                   orderBy: q => q.OrderBy(x => x.CreateAt),
                   page: page,
                   size: size
                    );

            return new BaseResponse<IPaginate<QuestionResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách câu hỏi thành công",
                Data = response
            };
        }

        public async Task<BaseResponse<IPaginate<QuestionResponse>>> GetQuestionsByTopic(Guid id, int page, int size)
        {
            if (page < 1 || size < 1)
            {
                throw new BadHttpRequestException("Số trang và số lượng trong trang phải lớn hơn hoặc bằng 1");
            }

            var response = await _unitOfWork.GetRepository<Question>().GetPagingListAsync(
                selector: q => new QuestionResponse
                {
                    Id = q.Id,
                    BookTopicId = q.BookTopicId,
                    Level = q.Level,
                    Content = q.Content,
                    Solution = q.Solution,
                    Image = q.Image,
                    CategoryId = q.CategoryId.Value,
                    CategoryName = q.Category.Name.ToString(),
                    CategoryGrade = q.Category.Grade.ToString(),
                    Answers = q.Answers.Select(a => new AnswerResponse
                    {
                        Id = a.Id,
                        Content = a.Content,
                        IsTrue = a.IsTrue.Value
                    }).ToList()
                },
                predicate: q => q.IsActive == true
                         && q.DeleteAt == null
                         && q.BookTopicId == id,
                include: q => q.Include(x => x.Category).Include(x => x.Answers),
                orderBy: q => q.OrderBy(x => x.CreateAt),
                page: page,
                size: size
            );

            return new BaseResponse<IPaginate<QuestionResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy câu hỏi theo chủ đề thành công",
                Data = response
            };
        }
        public async Task<BaseResponse<string>> DeleteQuestionById(Guid id)
        {
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var answerRepo = _unitOfWork.GetRepository<Answer>();

            var question = await questionRepo.SingleOrDefaultAsync(
                predicate: q => q.Id == id && q.IsActive == true && q.DeleteAt == null,
                include: q => q.Include(x => x.Answers)
            );


            if (question == null) throw new NotFoundException("Không tìm thấy câu hỏi");


            question.IsActive = false;
            question.DeleteAt = TimeUtil.GetCurrentSEATime();
            questionRepo.UpdateAsync(question);
            foreach (var answer in question.Answers)
            {
                answer.IsActive = false;
                answer.DeleteAt = TimeUtil.GetCurrentSEATime();
                answerRepo.UpdateAsync(answer);
            }

            await _unitOfWork.CommitAsync();

            return new BaseResponse<string>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Đã xóa câu hỏi và đáp án thành công",
                Data = id.ToString()
            };
        }

        public async Task<BaseResponse<QuestionResponse>> GetQuestion(Guid id)
        {
            var question = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                predicate: q => q.Id == id && q.IsActive == true && q.DeleteAt == null,
                include: q => q.Include(x => x.Category).Include(x => x.Answers)
            );

            if (question == null)
            {
                return new BaseResponse<QuestionResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy câu hỏi",
                    Data = null
                };
            }

            var response = new QuestionResponse
            {
                Id = question.Id,
                BookTopicId = question.BookTopicId,
                Level = question.Level,
                Content = question.Content,
                Solution = question.Solution,
                Image = question.Image,
                CategoryId = question.CategoryId.Value,
                CategoryName = question.Category?.ToString(),
                CategoryGrade = question.Category?.Grade.ToString(),
                Answers = question.Answers.Select(a => new AnswerResponse
                {
                    Id = a.Id,
                    Content = a.Content,
                    IsTrue = a.IsTrue.Value
                }).ToList()
            };

            return new BaseResponse<QuestionResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy câu hỏi thành công",
                Data = response
            };
        }

        public async Task<BaseResponse<List<QuestionSolutionResponse>>> GetAllQuestionSolution()
        {
            var questions = await _unitOfWork.GetRepository<Question>().GetListAsync(
         selector: q => new QuestionSolutionResponse
         {
             QuestionId = q.Id.ToString(),
             Question = q.Content,
             Answer = q.Solution,
             Topic = q.BookTopic.Name.ToString(),
             Grade = q.Category.Grade.ToString(),
             Chapter = q.BookTopic.BookChapter.Name.ToString()

         },
         predicate: q => q.IsActive == true && q.DeleteAt == null,
         include: q => q
             .Include(x => x.BookTopic)
             .Include(x => x.Category),
         orderBy: q => q.OrderBy(x => x.CreateAt)
     );

            return new BaseResponse<List<QuestionSolutionResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách câu hỏi và lời giải thành công",
                Data = questions.ToList()
            };
        }
    }
}
