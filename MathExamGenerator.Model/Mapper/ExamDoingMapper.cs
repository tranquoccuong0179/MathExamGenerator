using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.ExamDoing;
using MathExamGenerator.Model.Payload.Response.ExamDoing;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class ExamDoingMapper : Profile
    {
        public ExamDoingMapper()
        {
            CreateMap<CreateExamDoingRequest, ExamDoing>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.QuestionHistories, opt => opt.Ignore());

            CreateMap<ExamDoing, CreateExamDoingResponse>();
            CreateMap<ExamDoing, GetExamDoingResponse>()
                .ForMember(dest => dest.QuestionHistories, opt => opt.MapFrom(src => src.QuestionHistories));

            CreateMap<ExamDoing, ExamDoingOverviewResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Exam.Name));
        }
    }
}
