using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.TestHistory;
using MathExamGenerator.Model.Payload.Response.TestHistory;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class TestHistoryMapper : Profile
    {
        public TestHistoryMapper()
        {
            CreateMap<CreateTestHistoryRequest, TestHistory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.QuestionHistories, opt => opt.Ignore());

            CreateMap<TestHistory, CreateTestHistoryResponse>();
            CreateMap<TestHistory, GetTestHistoryResponse>()
                .ForMember(dest => dest.QuestionHistories, opt => opt.MapFrom(src => src.QuestionHistories));

            CreateMap<TestHistory, TestHistoryOverviewResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Name : src.Quiz != null ? src.Quiz.Name : null));

        }
    }
}
