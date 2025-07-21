using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ClubMemberDatabaseRepository : IClubMemberRepository
    {

        private readonly StakeholdersContext _dbContext;
        public ClubMemberDatabaseRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ClubMember GetByUserId(long id)
        {
            return _dbContext.ClubMembers.FirstOrDefault(cm => cm.UserId == id);
        }
    }
}
