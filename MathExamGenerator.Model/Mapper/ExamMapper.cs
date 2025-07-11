﻿using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Enum;
using MathExamGenerator.Model.Payload.Request.Exam;
using MathExamGenerator.Model.Payload.Response.Account;
using MathExamGenerator.Model.Payload.Response.Exam;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class ExamMapper : Profile
    {
        public ExamMapper()
        {
            CreateMap<CreateExamRequest, Exam>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Exam, CreateExamResponse>();

            CreateMap<Exam, GetExamResponse>();

            CreateMap<Answer, ExamAnswerResponse>();

            CreateMap<Question, QuestionWithAnswerResponse>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.CategoryGrade, o => o.MapFrom(s => s.Category.Grade))
                .ForMember(d => d.Answers, o => o.MapFrom(s => s.Answers.OrderBy(a => a.CreateAt)));

            CreateMap<Exam, ExamWithQuestionsResponse>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());
        }
    }
}
