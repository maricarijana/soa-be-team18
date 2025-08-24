using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class AppReviewService : CrudService<AppReviewDto, AppReview>, IAppReviewService
    {
        
        public AppReviewService(ICrudRepository<AppReview> repository, IMapper mapper) : base(repository, mapper) { }

      
    }
}
