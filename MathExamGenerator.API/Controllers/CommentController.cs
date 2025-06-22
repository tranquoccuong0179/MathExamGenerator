
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Comment;
using MathExamGenerator.Model.Payload.Request.Comment;

namespace MathExamGenerator.API.Controllers
{
    public class CommentController : BaseController<CommentController>
    {
        private readonly ICommentService _commentService;
        public CommentController(ILogger<CommentController> logger, ICommentService commentService) : base(logger)
        {
            _commentService = commentService;
        }

        [HttpPost(ApiEndPointConstant.Comment.CreateComment)]
        [ProducesResponseType(typeof(BaseResponse<CreateCommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateCommentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateCommentResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var response = await _commentService.CreateComment(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.Comment.UpdateComment)]
        [ProducesResponseType(typeof(BaseResponse<GetCommentRespose>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetCommentRespose>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<GetCommentRespose>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid id, [FromBody] UpdateCommentRequest request)
        {
            var response = await _commentService.UpdateCommnet(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Comment.DeleteComment)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            var response = await _commentService.DeleteComment(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
