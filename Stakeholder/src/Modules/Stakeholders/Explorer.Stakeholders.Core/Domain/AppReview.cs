using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public class AppReview : Entity
    {
        public long UserId { get; init; }
        public int Grade { get; init; }
        public string Comment { get; init; }
        public DateTime CreationTime { get; init; }

        public AppReview(long userId, int grade, string comment, DateTime creationTime)
        {
            UserId = userId;
            Grade = grade;
            Comment = comment;
            CreationTime = creationTime;
            Validate();
        }

        private void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid UserId");
            if (Grade <= 0 || Grade > 5) throw new ArgumentException("Invalid Grade");
            if (CreationTime > DateTime.Now) throw new ArgumentException("Invalide CrationTime");
        }
    }
}
