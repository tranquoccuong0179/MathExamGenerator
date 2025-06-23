
using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using MathExamGenerator.Model.Payload.Response.Quiz;
using MathExamGenerator.Model.Payload.Request.Quiz;
using MathExamGenerator.Model.Paginate;

namespace MathExamGenerator.API.Controllers
{
    public class QuizController : BaseController<QuizController>
    {
        private readonly IQuizService _quizService;
        public QuizController(ILogger<QuizController> logger, IQuizService quizService) : base(logger)
        {
            _quizService = quizService;
        }

        [HttpPost(ApiEndPointConstant.Quiz.CreateQuiz)]
        [ProducesResponseType(typeof(BaseResponse<CreateQuizResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CreateQuizResponse>), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
        {
            var response = await _quizService.CreateQuiz(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Quiz.GetAllQuiz)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetQuizResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllQuiz([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _quizService.GetAllQuiz(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Quiz.GetQuiz)]
        [ProducesResponseType(typeof(BaseResponse<GetQuizDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetQuizDetailResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuiz([FromRoute] Guid id)
        {
            var response = await _quizService.GetQuizDetail(id);
            return StatusCode(int.Parse(response.Status), response);
        }
    }
}
