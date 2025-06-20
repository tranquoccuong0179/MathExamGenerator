
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Subject;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Subject;
using MathExamGenerator.Model.Payload.Response.SubjectBook;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class SubjectController : BaseController<SubjectController>
    {
        private readonly ISubjectService _subjectService;
        private readonly ISubjectBookService _subjectBookService;
        public SubjectController(ILogger<SubjectController> logger, 
                                 ISubjectService subjectService,
                                 ISubjectBookService subjectBookService) : base(logger)
        {
            _subjectService = subjectService;
            _subjectBookService = subjectBookService;
        }

        [HttpPost(ApiEndPointConstant.Subject.CreateSubject)]
        [ProducesResponseType(typeof(BaseResponse<CreateSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateSubjectResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            var response = await _subjectService.CreateSubject(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetAllSubjects)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetSubjectResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllSubjects([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _subjectService.GetAllSubjects(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetSubject)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSubject([FromRoute] Guid id)
        {
            var response = await _subjectService.GetSubject(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetAllSubjectBookBySubject)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetSubjectBookResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetSubjectBookResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllSubjectBookBySubject([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _subjectBookService.GetAllSubjectBookBySubject(id, pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.Subject.UpdateSubject)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectBookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetSubjectBookResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubject([FromRoute] Guid id, [FromBody] UpdateSubjectRequest request)
        {
            var response = await _subjectService.UpdateSubject(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Subject.DeleteSubject)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubject([FromRoute] Guid id)
        {
            var response = await _subjectService.DeleteSubject(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
