namespace QAPortal.Business.Mappers;

using AutoMapper;
using QAPortal.Data.Entities;
using QAPortal.Shared.DTOs.QADtos;
using QAPortal.Shared.DTOs.UserDtos;

public class QAMapper : Profile
{
    public QAMapper()
    {


        CreateMap<QuestionsEntity, QuestionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy??0))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.IsEnded, opt => opt.MapFrom(src => src.IsEnded));

        CreateMap<AnswerDto, AnswersEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy));

        //Reverse mapping

        CreateMap<QuestionDto, QuestionsEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.IsEnded, opt => opt.MapFrom(src => src.IsEnded));

        CreateMap<AnswersEntity, AnswerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy));


        //Request DTo mappings
        CreateMap<QuestionRequestDto, QuestionsEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.IsEnded, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

        CreateMap<AnswerRequestDto, AnswersEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest=> dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());
    }
}