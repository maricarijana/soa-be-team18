using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ClubJoinRequestRepository : IClubJoinRequestRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ClubJoinRequestRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        
            public Result  Delete(ClubJoinRequest request)
            {
            //  _dbContext.ClubJoinRequests.Remove(request);
            //  _dbContext.SaveChanges(); // Obavezno sačuvaj promene u bazi
            try
            {
                // Proveri da li je request null
                if (request == null)
                {
                    return Result.Fail(FailureCode.NotFound)
                                 .WithError("Request does not exist.");
                }

                // Ukloni request iz baze
                _dbContext.ClubJoinRequests.Remove(request);
                _dbContext.SaveChanges(); // Sačuvaj promene

                // Vrati uspešan rezultat
                return Result.Ok();
            }
            catch (Exception e)
            {
                // Vrati grešku u slučaju izuzetka
                return Result.Fail("Request doesnt exist");
            }

        }
       

        public List<ClubJoinRequest> GetAll()
        {
            return _dbContext.ClubJoinRequests.ToList();
        }

        public ClubJoinRequest GetById(long requestId)
        {
            return _dbContext.ClubJoinRequests.FirstOrDefault(c => c.Id == requestId);
        }
    }
}
