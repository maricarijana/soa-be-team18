using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tours.Application.Dtos
{
    public class TourExecutionDto
    {
        public long Id { get; set; }
        public long TourId { get; set; }
        public long TouristId { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime? LastActivity { get; set; }

        public TourExecutionStatus Status { get; set; }
        public List<CompletedKeyPointDto> CompletedKeyPoints { get; set; } = new();

        public TourExecutionDto() { }

        //[JsonConstructor]
        public TourExecutionDto(
            long id,
            long tourId,
            long touristId,
            DateTime startedAt,
            DateTime? finishedAt,
            DateTime? lastActivity,
            TourExecutionStatus status,
            List<CompletedKeyPointDto> completedKeyPoints)
        {
            Id = id;
            TourId = tourId;
            TouristId = touristId;
            StartedAt = startedAt;
            FinishedAt = finishedAt;
            LastActivity = lastActivity;
            Status = status;
            CompletedKeyPoints = completedKeyPoints ?? new List<CompletedKeyPointDto>();
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

