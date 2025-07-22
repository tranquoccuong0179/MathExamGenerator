using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Payload.Request.Category;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Category;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class CategoryController : BaseController<CategoryController>
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) : base(logger)
        {
            _categoryService = categoryService;
        }


        [HttpGet(ApiEndPointConstant.Category.GetByGrade)]
        [ProducesResponseType(typeof(BaseResponse<List<CategorySelectResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetByGrade([FromRoute] string grade)
        {
            var response = await _categoryService.GetByGrade(grade);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpGet(ApiEndPointConstant.Category.GetAllGrades)]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllGrades()
        {
            var response = await _categoryService.GetAllGrades();
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpGet(ApiEndPointConstant.Category.GetCategoryById)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _categoryService.GetById(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Category.GetAllCategories)]
        [ProducesResponseType(typeof(BaseResponse<List<CategoryResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAll();
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPost(ApiEndPointConstant.Category.CreateCategory)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {
            var response = await _categoryService.Create(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpPut(ApiEndPointConstant.Category.UpdateCategory)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryRequest request)
        {
            var response = await _categoryService.Update(id, request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Category.DeleteCategory)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _categoryService.Delete(id);
            return StatusCode(int.Parse(response.Status), response);

        }
       
            
    }
}
