using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.BookTopic;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class BookTopicService : BaseService<BookTopicService>, IBookTopicService
    {
        public BookTopicService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<BookTopicService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<IPaginate<GetBookTopicResponse>>> GetAllBookTopic(int page, int size)
        {
            var response = await _unitOfWork.GetRepository<BookTopic>().GetPagingListAsync(
                selector: b => new GetBookTopicResponse
                {
                    Id = b.Id,
                    BookChapterId = b.BookChapterId,
                    Name = b.Name,
                    TopicNo = b.TopicNo
                },
                predicate: b => b.IsActive == true,
                orderBy: b => b.OrderBy(c => c.TopicNo),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetBookTopicResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Lấy danh sách chủ đề sách thành công",
                Data = response
            };
        }
    }
}
