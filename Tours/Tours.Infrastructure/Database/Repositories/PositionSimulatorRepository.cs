using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Core.Domain;
using Tours.Core.Domain.Execution;
using Microsoft.EntityFrameworkCore;



namespace Tours.Infrastructure.Database.Repositories;

public class PositionSimulatorRepository : CrudDatabaseRepository<PositionSimulator, ToursContext>, IPositionSimulatorRepository
{

    private readonly ToursContext _dbContext;

    public PositionSimulatorRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public PositionSimulator GetByTouristId(long touristId)
    {
        //return _dbContext.Positions.AsNoTracking()
        //                             .FirstOrDefault(ps => ps.TouristId == touristId);
        return _dbContext.Positions
                 .FirstOrDefault(ps => ps.TouristId == touristId);

        //        return _dbContext.Positions.Where(ps => ps.TouristId == touristId).FirstOrDefault(); }

    }
}
