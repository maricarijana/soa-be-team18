using AutoMapper;
using Tours.Application.Dtos;
using Tours.Application.Dtos.Shopping;
using Tours.Core.Domain;
using Tours.Core.Domain.Shopping;

namespace Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<KeyPointDto, KeyPoint>().ReverseMap();
        CreateMap<TourDto, Tour>().ReverseMap();
        //CreateMap<TourReviewDto, TourReview>().ReverseMap();
        CreateMap<TourReviewDto, TourReview>().ReverseMap();
        CreateMap<PositionSimulatorDto, PositionSimulator>().ReverseMap();
        CreateMap<TourDurationDto, TourDuration>().ReverseMap();
        //.ForMember(dest => dest.Images,
        //    opt => opt.MapFrom(src => string.Join(";", src.Images ?? new List<string>())))
        //.ReverseMap()
        //.ForMember(dest => dest.Images,
        //    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Images)
        //        ? new List<string>()
        //        : src.Images.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()));

        //CreateMap<ObjectDTO, Explorer.Tours.Core.Domain.Object>().ReverseMap();

        //CreateMap<CompletedKeyPointDto, CompletedKeyPoint>().ReverseMap();
        //CreateMap<TourExecutionDto, TourExecution>().IncludeAllDerived() //sluzi da budemo sigurni da lista keyPoints u dto bude ista kao i u klasi
        //    .ForMember(dest => dest.CompletedKeys, opt =>
        //    opt.MapFrom(src => src.CompletedKeys.Select((completedKey) =>
        //    new CompletedKeyPoint(completedKey.KeyPointId, completedKey.CompletedTime))));
        CreateMap<ShoppingCartItemCreationDto, ShoppingCartItem>().ReverseMap();
        CreateMap<ShoppingCartItemDto, ShoppingCartItem>().ReverseMap();
        CreateMap<ShoppingCartDto,ShoppingCart>().ReverseMap();
        CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
    }
}