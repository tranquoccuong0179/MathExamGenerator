
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.BookTopic;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Chapter;
using MathExamGenerator.Model.Payload.Response.SubjectBook;
using MathExamGenerator.Model.Payload.Request.SubjectBook;

namespace MathExamGenerator.API.Controllers
{
    public class SubjectBookController : BaseController<SubjectBookController>
    {
        private readonly IBookChapterService _bookChapterService;
        private readonly ISubjectBookService _subjectBookService;
        public SubjectBookController(ILogger<SubjectBookController> logger, 
                                     IBookChapterService bookChapterService,
                                     ISubjectBookService subjectBookService) : base(logger)
        {
            _bookChapterService = bookChapterService;
            _subjectBookService = subjectBookService;
        }

        [HttpPost(ApiEndPointConstant.SubjectBook.CreateSubjectBook)]
        [ProducesResponseType(typeof(BaseResponse<CreateSubjectBookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateSubjectBookResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateSubjectBookResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSubjectBook([FromForm] CreateSubjectBookRequest request)
        {
            var response = await _subjectBookService.CreateSubjectBook(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.SubjectBook.GetAllSubjectBooks)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetSubjectBookResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllSubjectBooks([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _subjectBookService.GetAllSubjectBooks(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.SubjectBook.GetSubjectBook)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectBookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectBookResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSubjectBook([FromRoute] Guid id)
        {
            var response = await _subjectBookService.GetSubjectBook(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.SubjectBook.GetAllChapterBySubjectBook)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetChapterResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetChapterResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllChapterBySubjectBook([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _bookChapterService.GetAllChapterBySubjectBook(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.SubjectBook.UpdateSubjectBook)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectBookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectBookResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateSubjectBook([FromRoute] Guid id, [FromBody] UpdateSubjectBookRequest request)
        {
            var response = await _subjectBookService.UpdateSubjectBook(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.SubjectBook.DeleteSubjectBook)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteSubjectBook([FromRoute] Guid id)
        {
            var response = await _subjectBookService.DeleteSubjectBook(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
