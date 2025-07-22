using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Exceptions;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.TestStorage;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
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
    public class TestStorageService : BaseService<TestStorageService>, ITestStorageService
    {
        public TestStorageService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<TestStorageService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<GetTestStorageResponse>> Create(CreateTestStorageRequest request)
        {        

            if (!request.ExamId.HasValue)
            {
                return new BaseResponse<GetTestStorageResponse>
                {
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Message = "Phải cung cấp ExamId",
                    Data = null
                };
            }

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.IsActive == true);

            if (account == null)
            {
                return new BaseResponse<GetTestStorageResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                    Data = null
                };
            }

            var exam = await _unitOfWork.GetRepository<Exam>().SingleOrDefaultAsync(
                predicate: x => x.Id == request.ExamId);

            if (exam == null)
            {
                return new BaseResponse<GetTestStorageResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy Exam.",
                    Data = null
                };
            }        

            var entity = _mapper.Map<TestStorage>(request);
            entity.AccountId = accountId;

            await _unitOfWork.GetRepository<TestStorage>().InsertAsync(entity);
            var isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<GetTestStorageResponse>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Đã xảy ra lỗi khi tạo dữ liệu lưu trữ.",
                    Data = null
                };
            }

            return new BaseResponse<GetTestStorageResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Tạo lưu trữ thành công.",
                Data = _mapper.Map<GetTestStorageResponse>(entity)
            };
        }

        public async Task<BaseResponse<bool>> Delete(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<TestStorage>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (entity == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy mục lưu trữ.",
                    Data = false
                };
            }

            entity.IsActive = false;
            entity.DeleteAt = TimeUtil.GetCurrentSEATime();
            entity.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<TestStorage>().UpdateAsync(entity);
            var isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Đã xảy ra lỗi khi xóa dữ liệu lưu trữ.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Xóa lưu trữ thành công.",
                Data = isSuccessfully
            };
        }

        public async Task<BaseResponse<IPaginate<GetTestStorageResponse>>> GetAll(int page, int size)
        {
            if (page < 1 || size < 1)
            {
                return new BaseResponse<IPaginate<GetTestStorageResponse>>
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
                return new BaseResponse<IPaginate<GetTestStorageResponse>>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy tài khoản.",
                    Data = null
                };
            }

            var result = await _unitOfWork.GetRepository<TestStorage>().GetPagingListAsync(
                selector: x => _mapper.Map<GetTestStorageResponse>(x),
                page: page,
                size: size,
                orderBy: q => q.OrderByDescending(x => x.CreateAt),
                predicate: x => x.IsActive == true && x.AccountId == account.Id,
                include: x => x.Include(x => x.Exam));

            return new BaseResponse<IPaginate<GetTestStorageResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách thành công.",
                Data = result
            };
        }

        public async Task<BaseResponse<GetTestStorageResponse>> GetById(Guid id)
        {
            var entity = await _unitOfWork.GetRepository<TestStorage>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true,
                include: x => x.Include(x => x.Exam));

            if (entity == null)
            {
                return new BaseResponse<GetTestStorageResponse>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy mục lưu trữ.",
                    Data = null
                };
            }

            return new BaseResponse<GetTestStorageResponse>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy thành công.",
                Data = _mapper.Map<GetTestStorageResponse>(entity)
            };
        }

        public async Task<BaseResponse<bool>> Update(Guid id, UpdateTestStorageRequest request)
        {
            var entity = await _unitOfWork.GetRepository<TestStorage>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.IsActive == true);

            if (entity == null)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Message = "Không tìm thấy mục lưu trữ.",
                    Data = false
                };
            }

            entity.Liked = request.Liked ?? entity.Liked;
            entity.Seen = request.Seen ?? entity.Seen;
            entity.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<TestStorage>().UpdateAsync(entity);
            var isSuccessfully = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessfully)
            {
                return new BaseResponse<bool>
                {
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "Đã xảy ra lỗi khi cập nhật dữ liệu lưu trữ.",
                    Data = isSuccessfully
                };
            }

            return new BaseResponse<bool>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Cập nhật lưu trữ thành công.",
                Data = isSuccessfully
            };
        }
    }
}
