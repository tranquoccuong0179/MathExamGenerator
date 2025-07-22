using MathExamGenerator.API.constant;
using MathExamGenerator.Model.Paginate;
using MathExamGenerator.Model.Payload.Request.Package;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Package;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MathExamGenerator.API.Controllers
{
    public class PackageController : BaseController<PackageController>
    {
        private readonly IPackageService _packageService;
        public PackageController(ILogger<PackageController> logger,IPackageService packageService) : base(logger)
        {
            _packageService = packageService;
        }

        [HttpPost(ApiEndPointConstant.Package.CreatePackage)]
        [ProducesResponseType(typeof(BaseResponse<GetPackageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetPackageResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest request)
        {
            var response = await _packageService.Create(request);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpDelete(ApiEndPointConstant.Package.DeletePackage)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeletePackage([FromRoute] Guid id)
        {
            var response = await _packageService.Delete(id);
            return StatusCode(int.Parse(response.Status), response);
        }
        [HttpGet(ApiEndPointConstant.Package.GetPackageById)]
        [ProducesResponseType(typeof(BaseResponse<GetPackageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetPackageResponse>), StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetPackage([FromRoute] Guid id)
        {
            var response = await _packageService.GetById(id);
            return StatusCode(int.Parse(response.Status), response);
        }

        [HttpGet(ApiEndPointConstant.Package.GetAllPackage)]
        [ProducesResponseType(typeof(BaseResponse<IPaginate<GetPackageResponse>>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllPackage([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _packageService.GetAll(pageNumber, pageSize);
            return StatusCode(int.Parse(response.Status), response);
        }


    }
}
