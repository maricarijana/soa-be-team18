using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubRepository
    {
        Club GetById(long id);

        List<Club> GetAll();
        List<long> GetUserIdsByClubId(long clubId);



    }
}
