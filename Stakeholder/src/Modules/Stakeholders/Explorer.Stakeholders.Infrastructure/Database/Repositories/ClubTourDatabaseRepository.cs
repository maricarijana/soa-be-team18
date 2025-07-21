using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ClubTourDatabaseRepository: IClubTourRepository
    {
        private readonly StakeholdersContext _dbContext;
        public ClubTourDatabaseRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ClubTour GetById(long clubTourId)
        {
            return _dbContext.ClubTours.FirstOrDefault(c => c.Id == clubTourId);
        }

        public List<ClubTour> GetAll()
        {
            return _dbContext.ClubTours.ToList();
        }
    }
}
