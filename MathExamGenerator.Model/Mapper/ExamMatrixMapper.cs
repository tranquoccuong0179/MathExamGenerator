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

            CreateMap<CreateMatrixSectionRequest, MatrixSection>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.MatrixSectionDetails, opt => opt.Ignore())
                .ForMember(dest => dest.ExamMatrixId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<CreateMatrixSectionDetailRequest, MatrixSectionDetail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.MatrixSectionId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<ExamMatrix, GetExamMatrixResponse>();

            CreateMap<ExamMatrix, ExamMatrixStructureResponse>()
                .ForMember(dest => dest.MatrixSections, opt => opt.MapFrom(src => src.MatrixSections.Where(s => s.IsActive == true)));

            CreateMap<MatrixSection, MatrixSectionStructureResponse>()
                .ForMember(dest => dest.MatrixSectionDetails, opt => opt.MapFrom(src => src.MatrixSectionDetails.Where(d => d.IsActive == true)));

            CreateMap<MatrixSectionDetail, MatrixSectionDetailResponse>();
        }
    }
}
