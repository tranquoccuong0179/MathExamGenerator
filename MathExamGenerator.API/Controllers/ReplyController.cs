
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.Reply;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Reply;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class ReplyController : BaseController<ReplyController>
    {
        private readonly IReplyService _replyService;
        public ReplyController(ILogger<ReplyController> logger, IReplyService replyService) : base(logger)
        {
            _replyService = replyService;
        }

        [HttpPut(ApiEndPointConstant.Reply.UpdateReply)]
        [ProducesResponseType(typeof(BaseResponse<GetReplyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetReplyResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<GetReplyResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateReply([FromRoute] Guid id, [FromBody] UpdateReplyRequest request)
        {
            var response = await _replyService.UpdateReply(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Reply.DeleteReply)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteReply([FromRoute] Guid id)
        {
            var response = await _replyService.DeleteReply(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
