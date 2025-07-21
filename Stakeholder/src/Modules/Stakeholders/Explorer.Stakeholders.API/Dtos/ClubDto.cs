using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ClubDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public long UserId { get; set; }
        public List<long> UserIds {get; set;}
        public string ImageBase64 {  get; set; }

        public List<ClubTags> Tags { get; set; }

        public enum ClubTags
        {
            Cycling,
            Culture,
            Adventure,
            FamilyFriendly,
            Nature,
            CityTour,
            Historical,
            Relaxation,
            Wildlife,
            NightTour,
            Beach,
            Mountains,
            Photography,
            Guided,
            SelfGuided
        }


    }
}
