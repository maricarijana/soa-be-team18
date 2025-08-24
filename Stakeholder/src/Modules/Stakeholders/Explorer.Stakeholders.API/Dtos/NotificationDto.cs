using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsRead { get; set; }
        public long UserId { get; set; }
        public NotificationType NotificationsType { get; set; }
        public long ResourceId { get; set; }



        public enum NotificationType
        {
            PROBLEM,
            MESSAGE,
            PURCHASE,
            OTHER
        }
    }
}