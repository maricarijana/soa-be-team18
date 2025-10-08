using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Application.Dtos;

public class PositionSimulatorDto
{
    public long Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public long TouristId { get; set; }

    public DateTime UpdatedAt { get; set; }
}
