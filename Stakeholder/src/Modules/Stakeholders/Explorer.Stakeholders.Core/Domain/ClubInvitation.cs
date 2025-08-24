using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubInvitation : Entity
    {
        
        public int ClubId { get; set; }
        public int MemberId { get; set; }
        public ClubStatus Status { get; set; }


        public ClubInvitation(int clubId, int memberId, ClubStatus status)
        {
            ClubId = clubId;
            MemberId = memberId;    
            Status = status;
            Validate();
        }

        private void Validate()
        {
            if (ClubId == 0) throw new ArgumentException("Invalid clubId");
            if (MemberId == 0) throw new ArgumentException("Invalid memberId");
        }
    }
    public enum ClubStatus
    {
        PROCESSING,
        ACCEPTED,
        DENIED
    }
}
