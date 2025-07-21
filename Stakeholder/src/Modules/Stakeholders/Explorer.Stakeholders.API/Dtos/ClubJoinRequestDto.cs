using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ClubJoinRequestDto
    {
        public long Id { get; set; }
        public long ClubId { get; set; }
        public long UserId { get; set; }
        public JoinRequestStatus Status {get; set;}
    }

    public enum JoinRequestStatus
    {
        PENDING,
        ACCEPTED,
        DENIED
    }
}
