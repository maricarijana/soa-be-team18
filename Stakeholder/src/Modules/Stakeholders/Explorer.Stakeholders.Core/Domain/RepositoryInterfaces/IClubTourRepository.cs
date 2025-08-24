using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubTourRepository
    {
        ClubTour GetById(long id);

        List<ClubTour> GetAll();
    }
}
