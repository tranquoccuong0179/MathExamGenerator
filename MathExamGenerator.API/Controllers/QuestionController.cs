
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Comment;

namespace MathExamGenerator.API.Controllers
{
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly ICommentService _commentService;
        public QuestionController(ILogger<QuestionController> logger, ICommentService commentService) : base(logger)
        {
            _commentService = commentService;
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
    }
}
