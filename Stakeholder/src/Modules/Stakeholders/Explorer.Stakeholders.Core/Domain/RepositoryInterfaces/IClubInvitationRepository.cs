using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubInvitationRepository
    {
        ClubInvitation Create(ClubInvitation clubInvitation);
        int GetMaxId();
        List<ClubInvitation> GetInvitationsByClubId(long clubId);
       
       
    }
}
