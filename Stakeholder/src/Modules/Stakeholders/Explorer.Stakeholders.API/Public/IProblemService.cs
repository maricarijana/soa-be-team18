using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IProblemService
    {
        Result<PagedResult<ProblemDTO>> GetPaged(int page, int pageSize);
        Result<ProblemDTO> Create(ProblemDTO problemDto);
        Result<ProblemDTO> Update(ProblemDTO problemDto);
        Result Delete(int id);
        Result<List<ProblemDTO>> GetByTouristId(long id);
        Result<List<ProblemDTO>> GetByTourId(long id);
        Result<ProblemDTO> PostComment(ProblemCommentDto commentDto);
        Result<ProblemDTO> UpdateActiveStatus(long id, bool isActive);
        Result<ProblemDTO> GetById(long id);
        Result DeleteProblem(int id);
    }
}
