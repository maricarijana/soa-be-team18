namespace Tours.Core.Domain
{
    public class Tour : Entity
    {
        public string Name { get;  set; }
        public string? Description { get;  set; }

        public string? Difficulty { get;  set; }

        public List<TourTags> Tags { get;  set; }
        public TourStatus Status { get;  set; }
        public double Price { get;  set; }
        public long UserId { get;  set; }

        public double LengthInKm { get;  set; }

        public DateTime PublishedTime { get; set; }

        public DateTime? ArchiveTime { get; set; }

        public List<long> EquipmentIds { get; set; }

        public ICollection<KeyPoint> KeyPoints { get; private set; } = new List<KeyPoint>();
        public List<TourDuration> Durations { get; private set; } = new List<TourDuration>();


        public Tour(string name, string? description, string? difficulty,List<TourTags> tags, long userId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            Name = name;
            Description = description;
            Difficulty = difficulty;
            if (tags == null || tags.Count == 0)
            { tags = new List<TourTags>(); }
            Tags = tags;
            if (userId <= 0)
                throw new ArgumentException("Invalid UserId. UserId must be a positive number.");
            UserId = userId;
            Status = TourStatus.Draft;
            //Price = price;
            Price = 0;
            LengthInKm = 0;
            PublishedTime = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            ArchiveTime = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            EquipmentIds = new List<long>();

        }

        public Tour()
        {
        }

        public void Archive(long authorId)
        {
            if (Status != TourStatus.Published) throw new ArgumentException("Tour must be published in order to be archived");
            IsAuthor(authorId);

            ArchiveTime = DateTime.UtcNow;
            Status = TourStatus.Archived;
        }

        //public void Publish(long authorId)
        //{
        //    if (Status == TourStatus.Archived) throw new ArgumentException("Archived tour can't be published");
        //    IsAuthor(authorId);


        //    Status = TourStatus.Published;
        //}
        public void Publish(long authorId)
        {
            if (Status == TourStatus.Archived)
                throw new ArgumentException("Archived tour can't be published");

            IsAuthor(authorId);

            // Uslov 1: osnovni podaci
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(Difficulty) || Tags.Count == 0)
                throw new InvalidOperationException("Tour must have name, description, difficulty and tags to be published.");

            //  Uslov 2: najmanje dve kljucne tacke
            if (KeyPoints == null || KeyPoints.Count < 2)
                throw new InvalidOperationException("Tour must have at least two key points to be published.");

            // Uslov 3: bar jedno definisano vreme trajanja ture
            if (Durations == null || Durations.Count == 0)
                throw new InvalidOperationException("Tour must have at least one defined duration to be published.");

            // Ako su svi uslovi ispunjeni:
            Status = TourStatus.Published;
            PublishedTime = DateTime.UtcNow;
        }

        public void AddDuration(TourDuration duration)
        {
            if (Durations.Any(d => d.Transport == duration.Transport))
                throw new InvalidOperationException("Duration for this transport type already exists.");

            Durations.Add(duration);
        }

        private void IsAuthor(long userId)
        {
            if (UserId != userId) throw new UnauthorizedAccessException("User is not the author of the tour");
        }

        public bool Reactivate(long authorId)
        {
            if (Status != TourStatus.Archived)
            {
                throw new ArgumentException("Tour must be archived in order to be reactivated");
            }

            IsAuthor(authorId);

            Status = TourStatus.Published;

            ArchiveTime = null;

            return true;
        }

        public void UpdateLength(double length)
        {
            LengthInKm = length;
        }


    }

    public enum TourStatus
    {
        Draft,
        Published,
        Archived
    }

    public enum TourTags
    {
        Cycling,
        Culture,
        Adventure,
        FamilyFriendly,
        Nature,
        CityTour,
        Historical,
        Relaxation,
        Wildlife,
        NightTour,
        Beach,
        Mountains,
        Photography,
        Guided,
        SelfGuided
    }
}
