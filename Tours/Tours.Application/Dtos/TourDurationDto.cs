using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Application.Dtos
{
    public class TourDurationDto
    {
        public TransportType Transport { get; set; }
        public int DurationInMinutes { get; set; }

        public TourDurationDto() { }

        public TourDurationDto(TransportType transport, int durationInMinutes)
        {
            if (durationInMinutes <= 0)
                throw new ArgumentException("Duration must be positive.");
            Transport = transport;
            DurationInMinutes = durationInMinutes;
        }
    }

    public enum TransportType
    {
        Walking,
        Bicycle,
        Car
    }
}

