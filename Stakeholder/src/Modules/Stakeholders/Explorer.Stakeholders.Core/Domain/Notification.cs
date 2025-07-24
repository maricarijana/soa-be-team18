using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Notification : Entity
    {

        public string Description { get; private set; }
        public DateTime CreationTime { get; private set; }
        public bool IsRead { get; private set; }
        public long UserId { get; private set; }
        public NotificationType NotificationsType { get; private set; }
        public long? ResourceId { get; private set; }
        //za id prijavljenog problema ili id poruke ili neka druga notifikacija-videcemo naknadno

        public Notification() { }

        public Notification(string description, DateTime creationTime, bool isRead, long userId, NotificationType notificationsType, long? resourceId)
        {
            Description = description;
            CreationTime = creationTime;
            //CreationTime=DateTime.UtcNow-da se psotavlja u trenutku kreiranja ali mislim da to moze i posle
            IsRead = isRead;
            //IsRead=false-da se inicijalno postavi
            UserId = userId;
            NotificationsType = notificationsType;
            ResourceId = resourceId;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (UserId <= 0) throw new ArgumentException("Invalid UserId");
            if (NotificationsType != NotificationType.OTHER && ResourceId <= 0)
                throw new ArgumentException("Invalid ResourceId for the specified NotificationType");

            if (CreationTime > DateTime.UtcNow) throw new ArgumentException("Invalid CreationTime");
        }


        public void MarkAsRead()
        {
            IsRead = true;
        }
        public void ProblemType()
        {
            NotificationsType = NotificationType.PROBLEM;
        }
        public void MessageType()
        {
            NotificationsType = NotificationType.MESSAGE;
        }
   
        public void PurchaseType()
        {
            NotificationsType = NotificationType.PURCHASE;
        }
        public enum NotificationType
        {
            PROBLEM,
            MESSAGE,
            PURCHASE,
            OTHER
        }
    }
}