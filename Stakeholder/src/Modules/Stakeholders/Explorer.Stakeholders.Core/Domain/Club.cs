using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Club:Entity
    {
        public string Name { get; private set; }
        public string Description { get;private set; }
        public string? Image {  get; private set; } 
        public long UserId { get; private set; }
        public List<long> UserIds { get; private set; } = new List<long>();
        public List<ClubTags> Tags { get; private set; }


        public Club() { }
        public Club(string name, string description, string? image, long userId, List<ClubTags> tags)
        {
            Name = name;
            Description = description;
            Image = image;
            UserId = userId;
            Tags = tags;
            Validate();

        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (UserId <= 0) throw new ArgumentException("Invalid UserId");
            if (Tags == null || Tags.Count == 0)
                Tags = new List<ClubTags>();
        }
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
