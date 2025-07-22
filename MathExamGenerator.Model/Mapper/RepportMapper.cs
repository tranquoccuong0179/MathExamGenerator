using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.Report;
using MathExamGenerator.Model.Payload.Response.Report;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class RepportMapper : Profile
    {
        public RepportMapper() {

            CreateMap<CreateReportRequest, Report>()
                .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                .ForMember(dest => dest.CreateAt, opt => TimeUtil.GetCurrentSEATime())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore());
            CreateMap<UpdateReportRequest, Report>()
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore());

            CreateMap<Report, GetReportResponse>();
        }
    }
}
