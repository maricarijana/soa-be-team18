using AutoMapper;
using Explorer.Stakeholders.API.Dtos;

using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Problems;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        CreateMap<AppReviewDto, AppReview>().ReverseMap();
        CreateMap<PersonDto, Person>().ReverseMap();
        CreateMap<ProblemDTO, Problem>().ReverseMap();
        CreateMap<ClubDto,Club>().ReverseMap();
        CreateMap<ClubInvitationDto, ClubInvitation>().ReverseMap();
        CreateMap<ClubJoinRequestDto, ClubJoinRequest>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<ProblemCommentDto,  ProblemComment>().ReverseMap();
        CreateMap<NotificationDto, Notification>().ReverseMap();
        CreateMap<ClubTourDto, ClubTour>().ReverseMap();
        CreateMap<ClubMemberDto, ClubMember>().ReverseMap();

        CreateMap<AccountDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)).ReverseMap();
    }
}