using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ClubTourDto
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int TourId { get; set; }
        public DateTime Date { get; set; }
        public int Discount { get; set; }
        public List<int> TouristIds { get; set; }
    }
}
