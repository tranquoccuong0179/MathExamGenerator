using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    namespace MathExamGenerator.Model.Mapper
    {
        public class QuestionHistoryMapper : Profile
        {
            public QuestionHistoryMapper()
            {
                CreateMap<QuestionHistory, CreateQuestionHistoryResponse>();
                CreateMap<QuestionHistory, GetQuestionHistoryResponse>();
                CreateMap<CreateQuestionHistoryRequest, QuestionHistory>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                    .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()))
                    .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(_ => TimeUtil.GetCurrentSEATime()));
            }
        }
    }
}
 