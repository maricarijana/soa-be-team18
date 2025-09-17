using AutoMapper;
using Tours.Application.Dtos;
using Tours.Core.Domain;

namespace Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<KeyPointDto, KeyPoint>().ReverseMap();
        CreateMap<TourDto, Tour>().ReverseMap();
        //CreateMap<ObjectDTO, Explorer.Tours.Core.Domain.Object>().ReverseMap();

        //CreateMap<CompletedKeyPointDto, CompletedKeyPoint>().ReverseMap();
        //CreateMap<TourExecutionDto, TourExecution>().IncludeAllDerived() //sluzi da budemo sigurni da lista keyPoints u dto bude ista kao i u klasi
        //    .ForMember(dest => dest.CompletedKeys, opt =>
        //    opt.MapFrom(src => src.CompletedKeys.Select((completedKey) =>
        //    new CompletedKeyPoint(completedKey.KeyPointId, completedKey.CompletedTime))));

    }
}