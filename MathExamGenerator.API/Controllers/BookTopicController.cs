
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.BookTopic;
using MathExamGenerator.Model.Payload.Request.BookTopic;

namespace MathExamGenerator.API.Controllers
{
    public class BookTopicController : BaseController<BookTopicController>
    {
        private readonly IBookTopicService _bookTopicService;
        private readonly IUploadService _uploadService;

        public BookTopicController(ILogger<BookTopicController> logger, IBookTopicService bookTopicService, IUploadService uploadService) : base(logger)
        {
            _bookTopicService = bookTopicService;
            _uploadService = uploadService;
        }

        [HttpPost(ApiEndPointConstant.BookTopic.CreateBookTopic)]
        [ProducesResponseType(typeof(BaseResponse<CreateBookTopicResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateBookTopicResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateBookTopic([FromBody] CreateBookTopicRequest request)
        {
            var response = await _bookTopicService.CreateBookTopic(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.BookTopic.GetAllBookTopic)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetBookTopicResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllBookTopic([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookTopicService.GetAllBookTopic(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.BookTopic.GetBookTopic)]
        [ProducesResponseType(typeof(BaseResponse<GetBookTopicResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetBookTopicResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetBookTopic([FromRoute] Guid id)
        {
            var response = await _bookTopicService.GetBookTopic(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.BookTopic.UpdateBookTopic)]
        [ProducesResponseType(typeof(BaseResponse<GetBookTopicResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetBookTopicResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateBookTopic([FromRoute] Guid id, [FromBody] UpdateBookTopicRequest request)
        {
            var response = await _bookTopicService.UpdateBookTopic(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.BookTopic.DeleteBookTopic)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteBookTopic([FromRoute] Guid id)
        {
            var response = await _bookTopicService.DeleteBookTopic(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
