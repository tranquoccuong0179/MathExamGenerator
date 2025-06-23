using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.TestStorage;
using MathExamGenerator.Model.Payload.Response.TestStorage;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class TestStorageMapper : Profile
    {
        public TestStorageMapper()
        {
            CreateMap<TestStorage, GetTestStorageResponse>();

            CreateMap<CreateTestStorageRequest, TestStorage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.AccountId, opt => opt.Ignore()); 
        }
    }
}
