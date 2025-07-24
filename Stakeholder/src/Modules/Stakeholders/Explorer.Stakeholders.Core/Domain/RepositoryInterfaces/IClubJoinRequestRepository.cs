using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubJoinRequestRepository
    {
        List<ClubJoinRequest> GetAll();
        ClubJoinRequest GetById(long requestId);
        Result Delete(ClubJoinRequest request);

    }
}
