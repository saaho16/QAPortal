namespace QAPortal.Business.Mappers;

using AutoMapper;
using QAPortal.Data.Entities;

using QAPortal.Shared.DTOs.UserDtos;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, UserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

        CreateMap<ApprovalEntity, ApprovalDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
            .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy))
            .ForMember(dest => dest.ApprovalFor, opt => opt.MapFrom(src => src.ApprovalFor));

        //Reverse mapping
        CreateMap<UserDto, UserEntity>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // PasswordHash should not be mapped from DTO


        CreateMap<ApprovalDto, ApprovalEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
            .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy))
            .ForMember(dest => dest.ApprovalFor, opt => opt.MapFrom(src => src.ApprovalFor));






        //Request DTo mappings

        CreateMap<UserRequestDto, UserEntity>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // PasswordHash should not be mapped from DTO

        CreateMap<ApprovalRequestDto, ApprovalEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
            .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy))
            .ForMember(dest => dest.ApprovalFor, opt => opt.MapFrom(src => src.ApprovalFor));
    }

}

