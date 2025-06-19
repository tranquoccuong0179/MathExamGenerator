
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.BookTopic;

namespace MathExamGenerator.API.Controllers
{
    public class BookTopicController : BaseController<BookTopicController>
    {
        private readonly IBookTopicService _bookService;

        public BookTopicController(ILogger<BookTopicController> logger, IBookTopicService bookService) : base(logger)
        {
            _bookService = bookService;
        }

        [HttpGet(ApiEndPointConstant.BookTopic.GetAllBookTopic)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookTopicResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllBookTopic([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookService.GetAllBookTopic(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
