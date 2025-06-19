using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Location;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MathExamGenerator.Service.Implement
{
    public class LocationService : BaseService<LocationService>, ILocationService
    {
        public LocationService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<LocationService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<List<GetLocationResponse>>> GetAllLocations()
        {
            var locations = await _unitOfWork.GetRepository<Location>().GetListAsync(
                selector: l => new GetLocationResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                },
                predicate: l => l.IsActive == true,
                orderBy: l => l.OrderBy(l => l.Name));

            return new BaseResponse<List<GetLocationResponse>>
            {
                Status = StatusCodes.Status200OK.ToString(),
                Message = "Danh sách thông tin các tỉnh, thành",
                Data = locations.ToList()
            };
        }
    }
}
