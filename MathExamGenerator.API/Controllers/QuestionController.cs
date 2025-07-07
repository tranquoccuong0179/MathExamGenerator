
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Comment;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Response.Question;

namespace MathExamGenerator.API.Controllers
{
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly ICommentService _commentService;
        private readonly IQuestionService _questionService;
        public QuestionController(ILogger<QuestionController> logger, ICommentService commentService, IQuestionService questionService) : base(logger)
        {
            _commentService = commentService;
            _questionService = questionService;
        }

        [HttpGet(ApiEndPointConstant.Question.GetAllCommentByQuestion)]
        [ProducesResponseType(typeof(BaseResponse<List<GetCommentRespose>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<GetCommentRespose>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllCommentByQuestion([FromRoute] Guid id)
        {
            var response = await _commentService.GetAllCommentByQuestion(id);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpGet(ApiEndPointConstant.Question.GetAllQuestion)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<QuestionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<QuestionResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllQuestion([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _questionService.GetAllQuestion(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Question.DeleteQuestionById)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteQuestionById([FromRoute] Guid id)
        {
            var response = await _questionService.DeleteQuestionById(id);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpGet(ApiEndPointConstant.Question.GetQuestionById)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<QuestionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<QuestionResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
        {
            var response = await _questionService.GetQuestion(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
