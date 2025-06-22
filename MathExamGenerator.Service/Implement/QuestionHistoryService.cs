using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
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

namespace MathExamGenerator.Service.Implement
{
    public class QuestionHistoryService : BaseService<QuestionHistoryService>, IQuestionHistoryService
    {
        public QuestionHistoryService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<QuestionHistoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateQuestionHistoryResponse>> Create(CreateQuestionHistoryRequest request)
        {
            var entity = _mapper.Map<QuestionHistory>(request);

            await _unitOfWork.GetRepository<QuestionHistory>().InsertAsync(entity);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<CreateQuestionHistoryResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình tạo lịch sử câu hỏi.",
                };
            }

            return new BaseResponse<CreateQuestionHistoryResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo lịch sử câu hỏi thành công",
                Data = _mapper.Map<CreateQuestionHistoryResponse>(entity)
            };
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<QuestionHistory>().SingleOrDefaultAsync(
            predicate: x => x.Id == id && x.IsActive == true);

            if (entity == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy dữ liệu.",
                    Data = false
                };
            }

            entity.IsActive = false;
            entity.DeleteAt = TimeUtil.GetCurrentSEATime();
            entity.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<QuestionHistory>().UpdateAsync(entity);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình xóa lịch sử câu hỏi.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa lịch sử câu hỏi thành công",
                Data = isSuccessfully
            };
        }

        public async Task<BaseResponse<IPaginate<GetQuestionHistoryResponse>>> GetAll(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<GetQuestionHistoryResponse>>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Trang hoặc kích thước không hợp lệ.",
                    Data = null
                };
            }

            var result = await _unitOfWork.GetRepository<QuestionHistory>().GetPagingListAsync(
                selector: x => _mapper.Map<GetQuestionHistoryResponse>(x),
                page: page,
                size: size,
                orderBy: q => q.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true);

            return new BaseResponse<IPaginate<GetQuestionHistoryResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<GetQuestionHistoryResponse>> GetById(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<QuestionHistory>().SingleOrDefaultAsync(
            predicate: x => x.Id == id && x.IsActive == true);

            if (entity == null)
            {
                return new BaseResponse<GetQuestionHistoryResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy lịch sử câu hỏi.",
                    Data = null
                };
            }

            return new BaseResponse<GetQuestionHistoryResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy lịch sử câu hỏi thành công.",
                Data = _mapper.Map<GetQuestionHistoryResponse>(entity)
            };
        }

        public async Task<BaseResponse<bool>> Update(Guid id, UpdateQuestionHistoryRequest request)
        {
            var entity = await _unitOfWork.GetRepository<QuestionHistory>().SingleOrDefaultAsync(
            predicate: x => x.Id == id && x.IsActive == true);

            if (entity == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy dữ liệu.",
                    Data = false
                };
            }

            entity.YourAnswer = request.YourAnswer ?? entity.YourAnswer;
            entity.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<QuestionHistory>().UpdateAsync(entity);

            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Một lỗi đã xảy ra trong quá trình cập nhật lịch sử câu hỏi.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật lịch sử câu hỏi thành công",
                Data = isSuccessfully
            };
        }
    }
}
