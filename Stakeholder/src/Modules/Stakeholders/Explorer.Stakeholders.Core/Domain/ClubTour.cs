using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubTour:Entity
    {

        public int ClubId { get; private set; }
        public int TourId { get; private set; }
        public DateTime Date { get; private set; }
        public int Discount { get; private set; }
        public List<int> TouristIds { get; private set; } = new List<int>();

        public ClubTour() { }

        public ClubTour(int clubId, int tourId, DateTime date, int discount)
        {
            ClubId = clubId;
            TourId = tourId;
            Date = date;
            Discount = discount;
            Validate();
        }

        private void Validate()
        {
            if (ClubId < 0) throw new ArgumentException("Invalid club");
            if (TourId < 0) throw new ArgumentException("Invalid tour");
            if (Discount <= 0) throw new ArgumentException("Invalid discount");

        }

    }

}

