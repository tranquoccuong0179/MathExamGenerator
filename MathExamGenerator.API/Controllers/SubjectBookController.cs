
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.BookTopic;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Chapter;

namespace MathExamGenerator.API.Controllers
{
    public class SubjectBookController : BaseController<SubjectBookController>
    {
        private readonly IBookChapterService _bookChapterService;
        public SubjectBookController(ILogger<SubjectBookController> logger, IBookChapterService bookChapterService) : base(logger)
        {
            _bookChapterService = bookChapterService;
        }

        [HttpGet(ApiEndPointConstant.SubjectBook.GetAllChapterBySubjectBook)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetChapterResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetChapterResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllChapterBySubjectBook([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookChapterService.GetAllChapterByBook(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
