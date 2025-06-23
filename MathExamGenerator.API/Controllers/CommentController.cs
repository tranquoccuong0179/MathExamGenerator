
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Comment;
using MathExamGenerator.Model.Payload.Request.Comment;
using MathExamGenerator.Model.Payload.Response.LikeComment;
using MathExamGenerator.Model.Payload.Response.Reply;
using MathExamGenerator.Model.Payload.Request.Reply;

namespace MathExamGenerator.API.Controllers
{
    public class CommentController : BaseController<CommentController>
    {
        private readonly ICommentService _commentService;
        private readonly ILikeCommentService _likeCommentService;
        private readonly IReplyService _replyService;
        public CommentController(ILogger<CommentController> logger, 
                                 ICommentService commentService, 
                                 ILikeCommentService likeCommentService,
                                 IReplyService replyService) : base(logger)
        {
            _commentService = commentService;
            _likeCommentService = likeCommentService;
            _replyService = replyService;
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

        [HttpPost(ApiEndPointConstant.Comment.LikeComment)]
        [ProducesResponseType(typeof(BaseResponse<LikeCommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<LikeCommentResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<LikeCommentResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> LikeComment([FromRoute] Guid id)
        {
            var response = await _likeCommentService.ToggleLike(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPost(ApiEndPointConstant.Comment.ReplyComment)]
        [ProducesResponseType(typeof(BaseResponse<CreateReplyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateReplyResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CreateReplyResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> ReplyComment([FromRoute] Guid id, [FromBody] CreateReplyRequest request)
        {
            var response = await _replyService.CreateReply(id, request);
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

        [HttpGet(ApiEndPointConstant.Comment.GetAllReplyByComment)]
        [ProducesResponseType(typeof(BaseResponse<List<GetReplyResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<List<GetReplyResponse>>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllReplyByComment([FromRoute] Guid id)
        {
            var response = await _replyService.GetAllReplyByComment(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
