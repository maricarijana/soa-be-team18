using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Core.Domain.Problems
{
    public class Problem : Entity
    {
        [Key]
        public long? Id { get; set; }
        public long UserId { get; set; }
        public long TourId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime Time { get; set; }
        public bool IsActive { get; set; }
        public List<ProblemComment>? Comments { get;  set; }

        public long Deadline { get; set; }

        public Problem(long userId, long tourId, string category, string description, int priority, DateTime time, bool isActive, long deadline)
        {

            UserId = userId;
            TourId = tourId;
            Category = category;
            Description = description;
            Priority = priority;
            Time = time;
            Validate();
            Comments = new List<ProblemComment>();
            IsActive = isActive;
            Deadline = deadline;
        }

        public void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid UserId");
            if (TourId == 0) throw new ArgumentException("Invalid TourId");
            if (Priority == 0) throw new ArgumentException("Invalid Priority");
            if (string.IsNullOrWhiteSpace(Category)) throw new ArgumentException("Invalid Category");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (Deadline < 0) throw new ArgumentException("Invalid Deadline");
        }

        public void PostComment(ProblemComment comment)
        {
            Comments ??= new List<ProblemComment>();
            Comments.Add(comment);
        }
    }
}
