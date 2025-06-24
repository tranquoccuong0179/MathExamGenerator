using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Request.ExamMatrix;
using MathExamGenerator.Model.Payload.Response.ExamMatrix;
using MathExamGenerator.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class ExamMatrixMapper : Profile
    {
        public ExamMatrixMapper()
        {

            CreateMap<CreateExamMatrixWithStructureRequest, ExamMatrix>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.MatrixSections, opt => opt.Ignore()) 
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<UpdateExamMatrixWithStructureRequest, ExamMatrix>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<ExamMatrix, GetExamMatrixResponse>();

            CreateMap<ExamMatrix, ExamMatrixStructureResponse>()
                .ForMember(dest => dest.MatrixSections, opt => opt.MapFrom(src => src.MatrixSections.Where(s => s.IsActive == true)));
        }
    }
}
