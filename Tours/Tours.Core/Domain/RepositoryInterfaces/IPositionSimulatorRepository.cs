using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Core.Domain.RepositoryInterfaces;

namespace Tours.Core.Domain.Execution;

public interface IPositionSimulatorRepository : ICrudRepository<PositionSimulator>
{
    PositionSimulator GetByTouristId(long touristId);
}
