
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.BookChapter;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.BookTopic;
using MathExamGenerator.Model.Payload.Response.Chapter;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class BookChapterController : BaseController<BookChapterController>
    {
        private IBookChapterService _bookChapterService;
        private IBookTopicService _bookTopicService;
        public BookChapterController(ILogger<BookChapterController> logger,
                                     IBookChapterService bookChapterService,
                                     IBookTopicService bookTopicService) : base(logger)
        {
            _bookChapterService = bookChapterService;
            _bookTopicService = bookTopicService;
        }

        [HttpPost(ApiEndPointConstant.BookChapter.CreateBookChapter)]
        [ProducesResponseType(typeof(BaseResponse<CreateChapterReponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateChapterReponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateBookChapter([FromBody] CreateBookChapterRequest request)
        {
            var response = await _bookChapterService.CreateChapter(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.BookChapter.GetAllBookChapters)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetChapterResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllBookChapters([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookChapterService.GetAllChapters(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.BookChapter.GetBookChapter)]
        [ProducesResponseType(typeof(BaseResponse<GetChapterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetChapterResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBookChapter([FromRoute] Guid id)
        {
            var response = await _bookChapterService.GetChapter(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.BookChapter.GetAllBookTopicByChapter)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookTopicResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookTopicResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllBookTopicByChapter([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookTopicService.GetAllBookTopicByChapter(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.BookChapter.UpdateBookChapter)]
        [ProducesResponseType(typeof(BaseResponse<GetChapterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetChapterResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBookChapter([FromRoute] Guid id, [FromBody] UpdateBookChapterRequest request)
        {
            var response = await _bookChapterService.UpdateChapter(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.BookChapter.DeleteBookChapter)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteBookChapter([FromRoute] Guid id)
        {
            var response = await _bookChapterService.DeleteChapter(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
