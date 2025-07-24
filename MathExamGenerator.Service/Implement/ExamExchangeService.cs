using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.ExamExchange;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.ExamExchange;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.Identity.Client;
using MathExamGenerator.Model.Enum;
using Google.Apis.Drive.v3.Data;
using Microsoft.IdentityModel.Tokens;
namespace MathExamGenerator.Service.Implement
{
    public class ExamExchangeService : BaseService<ExamExchangeService>, IExamExchangeService
    {
        public ExamExchangeService(
           IUnitOfWork<MathExamGeneratorContext> unitOfWork,
           ILogger<ExamExchangeService> logger,
           IMapper mapper,
           IHttpContextAccessor accessor)
           : base(unitOfWork, logger, mapper, accessor)
        {
        }


        public async Task<BaseResponse<ExamExchangeResponse>> Create(ExamExchangeRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) &&
                                a.IsActive == true &&
                                a.Role == RoleEnum.STAFF.ToString())
            ?? throw new NotFoundException("Chỉ Staff mới được phép thực hiện hành động này");

            if (request.CategoryId == null || request.CategoryId == Guid.Empty)
                throw new NotFoundException("Vui lòng chọn danh mục cho phiếu đề thi");

            if (request.Questions == null || request.Questions.Count == 0)
                throw new NotFoundException("Không có câu hỏi nào được gửi lên.");

            if (request.Questions.Any(q => q.BookTopicId == null || q.BookTopicId == Guid.Empty))
                throw new NotFoundException("Vui lòng chọn chủ đề cho tất cả các câu hỏi");


            var examRepo = _unitOfWork.GetRepository<ExamExchange>();
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var questionRepo = _unitOfWork.GetRepository<Question>();

            var examEntity = _mapper.Map<ExamExchange>(request);

            examEntity.AccountId = accountId;
            examEntity.Status = ExamExchangeEnum.Pending.ToString();

            var category = await categoryRepo.GetByConditionAsync(
                predicate: c => c.Id == request.CategoryId && c.IsActive == true)
                ?? throw new NotFoundException("Không tìm thấy danh mục");

          

            var normalizedMap = examEntity.Questions
                .Select(q => new { Question = q, Norm = q.Content.Trim().ToLower() })
                .ToList();
            var norms = normalizedMap.Select(x => x.Norm).ToHashSet();

            var existedContents = await questionRepo.GetListAsync(
                selector: x => x.Content,
                predicate: x => norms.Contains(x.Content.Trim().ToLower()) &&
                                x.CategoryId == category.Id &&
                                x.DeleteAt == null);

            var existedSet = existedContents
                .Select(c => c.Trim().ToLower())
                .ToHashSet();

            var uniqueQuestions = new List<Question>();
            foreach (var item in normalizedMap)
            {
                if (!existedSet.Contains(item.Norm))
                {
                    var q = item.Question;
                    q.CategoryId = category.Id;
                    q.IsActive = false;
                    q.CreateAt = TimeUtil.GetCurrentSEATime();
                    foreach (var a in q.Answers)
                        a.IsActive = false;

                    uniqueQuestions.Add(q);
                }
            }

            if (uniqueQuestions.Count == 0)
            {
                throw new NotFoundException("Tất cả câu hỏi đều đã tồn tại trong hệ thống.");
            }

            examEntity.Questions = uniqueQuestions;

            await examRepo.InsertAsync(examEntity);
            var result = await _unitOfWork.CommitAsync() > 0;
            if (!result)
                throw new Exception("Lỗi trong quá trình lưu câu hỏi");

            var response = _mapper.Map<ExamExchangeResponse>(examEntity);
            response.CategoryId = category.Id;
            response.CategoryName = category.Name;
            response.CategoryGrade = category.Grade;

            foreach (var q in response.Questions)
            {
                q.CategoryId = category.Id;
                q.CategoryName = category.Name;
                q.CategoryGrade = category.Grade;
            }

            return new BaseResponse<ExamExchangeResponse>
            {
                Status = StatusCodes.Status201Created.ToString(),
                Message = "Tạo Phiếu thành công",
                Data = response
            };
        }


        public async Task<BaseResponse<IPaginate<GetExamExchangeResponse>>> GetByStaff(int page, int size)
        {

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
             predicate: a => a.Id.Equals(accountId) &&
                             a.IsActive == true &&
                             a.Role == RoleEnum.STAFF.ToString())
             ?? throw new NotFoundException("Chỉ Staff mới được phép thực hiện hành động này");

            var exams = await _unitOfWork.GetRepository<ExamExchange>().GetPagingListAsync(
            selector: e => new GetExamExchangeResponse
            {
                ExamExchangeId = e.Id,
                Status = e.Status,
                CreateAt = e.CreateAt,
                QuestionCount = e.Questions.Count,
                CategoryName = e.Questions.FirstOrDefault() != null
                                ? e.Questions.First().Category.Name
                                : null,
                CategoryGrade = e.Questions.FirstOrDefault() != null
                                ? e.Questions.First().Category.Grade
                                : null
            },
            predicate: e => e.AccountId == accountId && e.IsActive == true && e.DeleteAt ==null,
            orderBy: q => q.OrderByDescending(e => e.CreateAt),
            include: q => q
                        .Include(e => e.Questions)
                            .ThenInclude(q => q.Category),
            page: page,
            size: size);

            return new BaseResponse<IPaginate<GetExamExchangeResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách phiếu thành công",
                Data = exams
            };
        }


        public async Task<BaseResponse<IPaginate<GetExamExchangeStaffResponse>>> GetAllStaff(int page, int size)
        {
            var exams = await _unitOfWork.GetRepository<ExamExchange>().GetPagingListAsync(
                selector: e => new GetExamExchangeStaffResponse
                {
                    ExamExchangeId = e.Id,
                    Status = e.Status,
                    CreateAt = e.CreateAt,
                    QuestionCount = e.Questions.Count,
                    CategoryName = e.Questions.FirstOrDefault() != null
                          ? e.Questions.First().Category.Name
                          : null,
                    CategoryGrade = e.Questions.FirstOrDefault() != null
                          ? e.Questions.First().Category.Grade
                          : null,
                    StaffId = e.AccountId.Value,
                    StaffName = e.Account.FullName
                },
                predicate: e => e.IsActive == true &&
                                e.Account != null &&
                                e.Account.Role == RoleEnum.STAFF.ToString() && e.DeleteAt ==null,
                orderBy: q => q.OrderByDescending(e => e.CreateAt),
                include: q => q
                            .Include(e => e.Questions)
                                .ThenInclude(q => q.Category)
                            .Include(e => e.Account),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetExamExchangeStaffResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách phiếu thành công",
                Data = exams
            };
        }


        public async Task<BaseResponse<ExamExchangeResponse>> GetExamChange(Guid examchangeId)
        {

            var exam = await _unitOfWork.GetRepository<ExamExchange>()
                .SingleOrDefaultAsync(
                    predicate: e => e.Id == examchangeId &&
                                    e.IsActive == true && e.DeleteAt ==null,
                    include: q => q
                        .Include(e => e.Questions)
                            .ThenInclude(q => q.Answers)
                        .Include(e => e.Questions)
                            .ThenInclude(q => q.Category));

            if (exam == null)
                throw new NotFoundException("Không tìm thấy đề thi");

            var data = _mapper.Map<ExamExchangeResponse>(exam);

            var first = data.Questions.FirstOrDefault();
            if (first != null)
            {
                data.CategoryName = first.CategoryName;
                data.CategoryGrade = first.CategoryGrade;
                data.CategoryId = first.CategoryId;

            }

            return new BaseResponse<ExamExchangeResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy chi tiết đề thành công",
                Data = data
            };
        }

        public async Task<BaseResponse<ExamExchangeResponse>> Update(Guid examchangeId, UpdateExamEchangeRequest request)
        {
            if (request.Questions == null || request.Questions.Count == 0)
                throw new NotFoundException("Không có câu hỏi nào được gửi lên.");

            var examRepo = _unitOfWork.GetRepository<ExamExchange>();
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var answerRepo = _unitOfWork.GetRepository<Answer>();
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var topic = _unitOfWork.GetRepository<BookTopic>();

            var exam = await examRepo.SingleOrDefaultAsync(
                predicate: e => e.Id == examchangeId && e.IsActive == true && e.Status != "Approved",
                include: q => q.Include(e => e.Questions).ThenInclude(q => q.Answers))
                ?? throw new NotFoundException("Không tìm thấy phiếu");

            var category = await categoryRepo.SingleOrDefaultAsync(
                predicate: c => c.Id == request.CategoryId && c.IsActive == true)
                ?? throw new NotFoundException("Không tìm thấy danh mục");

           

            var requestNorms = request.Questions
                .Where(q => !string.IsNullOrWhiteSpace(q.Content))
                .Select(q => q.Content.Trim().ToLower())
                .ToHashSet();

            var existedContents = await questionRepo.GetListAsync(
                selector: x => x.Content,
                predicate: x => x.DeleteAt == null);

            var existedNorms = existedContents
                .Select(x => x.Trim().ToLower())
                .ToHashSet();

            var keepQIds = new HashSet<Guid>();

            foreach (var rq in request.Questions)
            {
            
                var normContent = rq.Content?.Trim().ToLower();

                if (!string.IsNullOrEmpty(normContent) && !rq.Id.HasValue && existedNorms.Contains(normContent))
                    continue;

                var qEntity = rq.Id.HasValue
                    ? exam.Questions.FirstOrDefault(q => q.Id == rq.Id.Value)
                    : null;

                if (qEntity == null)
                {
                    qEntity = _mapper.Map<Question>(rq);
                    qEntity.Id = Guid.NewGuid();
                    qEntity.CreateAt = TimeUtil.GetCurrentSEATime();
                    qEntity.IsActive = false;
                    qEntity.CategoryId = category.Id;
                    qEntity.ExamExchangeId = exam.Id;
                    qEntity.BookTopicId = rq.BookTopicId; 
                    exam.Questions.Add(qEntity);
                    _unitOfWork.Context.Entry(qEntity).State = EntityState.Added;
                }
                else
                {
                    qEntity.Content = rq.Content;
                    qEntity.Image = rq.Image;
                    qEntity.Level = rq.Level;
                    qEntity.Solution = rq.Solution;
                    qEntity.BookTopicId = rq.BookTopicId;
                    qEntity.UpdateAt = TimeUtil.GetCurrentSEATime();
                    qEntity.CategoryId = category.Id;
                }

                keepQIds.Add(qEntity.Id);

                if (rq.Answers == null || rq.Answers.Count == 0)
                    throw new NotFoundException($"Câu hỏi '{rq.Content}' thiếu đáp án.");

                var keepAIds = new HashSet<Guid>();

                foreach (var ra in rq.Answers)
                {
                    var aEntity = ra.Id.HasValue
                        ? qEntity.Answers.FirstOrDefault(a => a.Id == ra.Id.Value)
                        : null;

                    if (aEntity == null)
                    {
                        aEntity = new Answer
                        {
                            Id = Guid.NewGuid(),
                            Content = ra.Content,
                            Image = ra.Image,
                            IsTrue = ra.IsTrue,
                            CreateAt = TimeUtil.GetCurrentSEATime(),
                            IsActive = false
                        };
                        qEntity.Answers.Add(aEntity);
                        _unitOfWork.Context.Entry(aEntity).State = EntityState.Added;
                    }
                    else
                    {
                        aEntity.Content = ra.Content;
                        aEntity.Image = ra.Image;
                        aEntity.IsTrue = ra.IsTrue;
                        aEntity.UpdateAt = TimeUtil.GetCurrentSEATime();
                        _unitOfWork.Context.Update(aEntity);
                    }

                    keepAIds.Add(aEntity.Id);
                }

                var delAns = qEntity.Answers
                     .Where(a => !keepAIds.Contains(a.Id))
                     .ToList();

                foreach (var a in delAns)
                {
                    _unitOfWork.Context.Remove(a);
                }
            }

            var delQs = exam.Questions
                 .Where(q => !keepQIds.Contains(q.Id))
                 .ToList();

            foreach (var q in delQs)
            {
                foreach (var a in q.Answers.ToList())
                    _unitOfWork.Context.Remove(a);

                _unitOfWork.Context.Remove(q);
            }

            if (exam.Status == "Rejected") exam.Status = "Pending";
            exam.UpdateAt = TimeUtil.GetCurrentSEATime();

            var result = await _unitOfWork.CommitAsync() > 0;
            if (!result)
                throw new Exception("Không có thay đổi nào được thực hiện.");

            var data = _mapper.Map<ExamExchangeResponse>(exam);
            data.CategoryId = category.Id;
            data.CategoryName = category.Name;
            data.CategoryGrade = category.Grade;

            foreach (var q in data.Questions)
            {
                q.CategoryId = category.Id;
                q.CategoryName = category.Name;
                q.CategoryGrade = category.Grade;
            }

            return new BaseResponse<ExamExchangeResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật phiếu thành công",
                Data = data
            };
        }



        public async Task<BaseResponse<bool>> Delete(Guid examchangeId)
        {
            var examRepo = _unitOfWork.GetRepository<ExamExchange>();

            var exam = await examRepo.SingleOrDefaultAsync(
                predicate: e => e.Id == examchangeId && e.IsActive == true,
                include: q => q.Include(e => e.Questions).ThenInclude(q => q.Answers));

            if (exam == null)
                throw new NotFoundException("Không tìm thấy phiếu");

            if (exam.Status == "Approved")
                throw new InvalidOperationException("Phiếu đã được duyệt, không thể xoá");

            foreach (var q in exam.Questions)
            {
                foreach (var a in q.Answers)
                {
                    a.IsActive = false;
                    a.UpdateAt = TimeUtil.GetCurrentSEATime();
                    a.DeleteAt = TimeUtil.GetCurrentSEATime();

                    _unitOfWork.GetRepository<Answer>().UpdateAsync(a);
                }

                q.IsActive = false;
                q.UpdateAt = TimeUtil.GetCurrentSEATime();
                q.DeleteAt = TimeUtil.GetCurrentSEATime();
                _unitOfWork.GetRepository<Question>().UpdateAsync(q);

            }

            exam.IsActive = false;
            exam.UpdateAt = TimeUtil.GetCurrentSEATime();
            exam.DeleteAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<ExamExchange>().UpdateAsync(exam);

            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xoá Phiếu thành công",
                Data = true
            };
        }


        public async Task<BaseResponse<bool>> ApproveExamExchange(Guid id, ExamExchangeEnum? status)
        {
            var examRepo = _unitOfWork.GetRepository<ExamExchange>();

            var exam = await examRepo.SingleOrDefaultAsync(
                predicate: e => e.Id == id && e.DeleteAt == null,
                include: q => q.Include(e => e.Questions)
                               .ThenInclude(q => q.Answers))
                ?? throw new NotFoundException("Không tìm thấy phiếu đề.");

            if (exam.Status == status.ToString())
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Trạng thái mới trùng với trạng thái hiện tại.",
                    Data = false
                };
            }

            // Cập nhật trạng thái phiếu đề
            exam.Status = status.ToString();
            exam.UpdateAt = TimeUtil.GetCurrentSEATime();
            _unitOfWork.GetRepository<ExamExchange>().UpdateAsync(exam);

            if (status == ExamExchangeEnum.Approved)
            {
                foreach (var q in exam.Questions)
                {
                    if (q.IsActive != true)
                    {
                        q.IsActive = true;
                        q.UpdateAt = TimeUtil.GetCurrentSEATime();
                        _unitOfWork.GetRepository<Question>().UpdateAsync(q);
                    }

                    foreach (var a in q.Answers)
                    {
                        if (a.IsActive != true)
                        {
                            a.IsActive = true;
                            a.UpdateAt = TimeUtil.GetCurrentSEATime();
                            _unitOfWork.GetRepository<Answer>().UpdateAsync(a);
                        }
                    }
                }
            }
            else if (status == ExamExchangeEnum.Rejected)
            {
                foreach (var q in exam.Questions)
                {
                    if (q.IsActive != false)
                    {
                        q.IsActive = false;
                        q.UpdateAt = TimeUtil.GetCurrentSEATime();
                        _unitOfWork.GetRepository<Question>().UpdateAsync(q);
                    }

                    foreach (var a in q.Answers)
                    {
                        if (a.IsActive != false)
                        {
                            a.IsActive = false;
                            a.UpdateAt = TimeUtil.GetCurrentSEATime();
                            _unitOfWork.GetRepository<Answer>().UpdateAsync(a);
                        }
                    }
                }
            }

            var result = await _unitOfWork.CommitAsync();

            if (result == 0)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Không có thay đổi nào được ghi nhận.",
                    Data = false
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Phê duyệt phiếu thành công",
                Data = true
            };
        }


    }
}
