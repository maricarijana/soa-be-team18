using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Core.Domain
{
    public class TourDuration:Entity
    {
        public TransportType Transport { get; private set; }
        public int DurationInMinutes { get; private set; }

        public TourDuration(TransportType transport, int durationInMinutes)
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
