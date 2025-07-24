using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;
using Microsoft.Extensions.Options;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubJoinRequest : Entity
    {
        public long UserId { get; set; }
        public long ClubId { get; set; }
        public JoinRequestStatus Status { get; set; }

        public ClubJoinRequest(long userId, long clubId, JoinRequestStatus status) 
        {
            UserId = userId;
            ClubId = clubId;
            Status = status;
            Validate();
        }

        private void Validate()
        {
            if(UserId == 0) throw new ArgumentException("Invalid UserId");
            if(ClubId == 0) throw new ArgumentException("Invalid ClubId");
        }

    }

    public enum JoinRequestStatus
    {
        PENDING,
        ACCEPTED,
        DENIED
    }
}
