using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.Package;
using MathExamGenerator.Model.Payload.Response.Package;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class PackageMapper : Profile
    {
        public PackageMapper()
        {
            CreateMap<Package, GetPackageResponse>();
            CreateMap<CreatePackageRequest, Package>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore());
            CreateMap<UpdatePackageRequest, Package>()
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore());
        }

    }
}
