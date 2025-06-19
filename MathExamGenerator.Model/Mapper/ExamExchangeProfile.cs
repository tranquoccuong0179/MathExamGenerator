using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.ExamExchange;
using MathExamGenerator.Model.Payload.Response.ExamExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class ExamExchangeProfile : Profile
    {
        public ExamExchangeProfile()
        {
            //request
            CreateMap<AnswerReQuest, Answer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

            CreateMap<QuestionRequest, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

            CreateMap<ExamExchangeRequest, ExamExchange>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            //respone
            CreateMap<Answer, AnswerResponse>();
            CreateMap<Question, QuestionResponse>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.CategoryGrade, o => o.MapFrom(s => s.Category.Grade))
                .ForMember(d => d.Answers, o => o.MapFrom(s => s.Answers.OrderBy(a => a.CreateAt)));
            CreateMap<ExamExchange, ExamExchangeResponse>()
                .ForMember(d => d.Questions, o => o.MapFrom(s => s.Questions.OrderBy(q => q.CreateAt)))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Questions.FirstOrDefault().Category.Name))
                .ForMember(d => d.CategoryGrade, o => o.MapFrom(s => s.Questions.FirstOrDefault().Category.Grade));
        }
    }
}
