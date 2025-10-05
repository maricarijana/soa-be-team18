using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Core.Domain
{
    public class TourReview : Entity
    {
        public long IdTour { get; private set; }
        public long IdTourist { get; private set; }
        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public DateTime? DateTour { get; private set; }
        public DateTime? DateComment { get; private set; }
        public List<string> Images { get; private set; } = new();


        public TourReview(long idTour, long idTourist, int rating, string comment, DateTime? dateTour, DateTime? dateComment, List<string> images)
        {
            IdTour = idTour != 0 ? idTour : throw new ArgumentException("Invalid idTour");
            IdTourist = idTourist != 0 ? idTourist : throw new ArgumentException("Invalid idTourist");
            Rating = rating >= 1 && rating <= 5 ? rating : throw new ArgumentException("Invalid Rating");
            Comment = string.IsNullOrWhiteSpace(comment) ? "" : comment;
            DateTour = dateTour ?? throw new ArgumentNullException("Invalid Date");
            DateComment = dateComment ?? throw new ArgumentNullException("Invalid Date");
            Images = images ?? new List<string>();
        }


        public TourReview()
        {

        }
    }
}
