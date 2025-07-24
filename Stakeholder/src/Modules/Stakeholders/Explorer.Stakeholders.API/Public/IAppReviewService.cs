using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System.Data;

namespace Explorer.Stakeholders.API.Public
{
    public interface IAppReviewService
    {
        Result<AppReviewDto> Create(AppReviewDto appReview);
        Result<AppReviewDto> Update(AppReviewDto appReview);
        Result<PagedResult<AppReviewDto>> GetPaged(int page, int pageSize);
        Result<AppReviewDto> Get(int id);
    }
}
