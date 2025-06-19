using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Payload.Response;
using MathExamGenerator.Model.Payload.Response.Location;

namespace MathExamGenerator.Service.Interface
{
    public interface ILocationService
    {
        Task<BaseResponse<List<GetLocationResponse>>> GetAllLocations();
    }
}
