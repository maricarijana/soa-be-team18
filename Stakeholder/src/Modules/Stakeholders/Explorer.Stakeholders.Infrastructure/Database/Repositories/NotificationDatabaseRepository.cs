using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Explorer.Stakeholders.Core.Domain;



namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class NotificationDatabaseRepository : INotificationRepository
    {
        private readonly StakeholdersContext _dbContext;

        public NotificationDatabaseRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }


        public List<Notification> GetAllByUser(long userId)
        {
            return _dbContext.Notification
                .Where(n => n.UserId == userId)
                .ToList();
        }
        public List<Notification> GetUnreadByUser(long userId)
        {
            return _dbContext.Notification
                .Where(n => n.UserId == userId && n.IsRead == false)
                .ToList();
        }






    }

}