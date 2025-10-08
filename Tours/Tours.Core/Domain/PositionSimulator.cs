using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Core.Domain;

public class PositionSimulator : Entity
{
    public double Longitude { get; private set; }
    public double Latitude { get; private set; }

    public long TouristId { get; private set; }
    public DateTime UpdatedAt { get; private set; }


    public PositionSimulator(double latitude, double longitude, long touristId)
    {
        Validate(latitude, longitude);
        Latitude = latitude;
        Longitude = longitude;
        TouristId = touristId;
        UpdatedAt = DateTime.UtcNow;
    }

    private void Validate(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude value.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude value.");

    }
    public void Update(double latitude, double longitude)
    {
        Validate(latitude, longitude);
        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTime.UtcNow; 
    }
}
