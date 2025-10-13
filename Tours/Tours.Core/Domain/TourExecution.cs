using System;
using System.Collections.Generic;
using System.Linq;

namespace Tours.Core.Domain
{
    public class TourExecution : Entity
    {
        public long TourId { get; private set; }
        public long TouristId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public DateTime? LastActivity { get; private set; }
        public TourExecutionStatus Status { get; private set; }

        public List<CompletedKeyPoint> CompletedKeyPoints { get; private set; } = new();

        protected TourExecution() { }

        public TourExecution(long tourId, long touristId)
        {
            TourId = tourId;
            TouristId = touristId;
            StartedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
            Status = TourExecutionStatus.Active;
        }

        public void CompleteTour()
        {
            ValidateActive();
            Status = TourExecutionStatus.Completed;
            FinishedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
        }

        public void AbandonTour()
        {
            ValidateActive();
            Status = TourExecutionStatus.Abandoned;
            FinishedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
        }

        public void UpdateLastActivity()
        {
            LastActivity = DateTime.UtcNow;
        }

        public CompletedKeyPoint CompleteKeyPoint(long keyPointId)
        {
            ValidateActive();

            if (CompletedKeyPoints.Any(k => k.KeyPointId == keyPointId))
                throw new InvalidOperationException($"Key point {keyPointId} already completed.");

            var completed = new CompletedKeyPoint(keyPointId, DateTime.UtcNow);
            CompletedKeyPoints.Add(completed);
            UpdateLastActivity();
            return completed;
        }

        private void ValidateActive()
        {
            if (Status != TourExecutionStatus.Active)
                throw new InvalidOperationException("Tour execution must be active.");
        }
    }

    public enum TourExecutionStatus
    {
        NotStarted = 0,
        Active = 1,
        Completed = 2,
        Abandoned = 3
    }

    
}
