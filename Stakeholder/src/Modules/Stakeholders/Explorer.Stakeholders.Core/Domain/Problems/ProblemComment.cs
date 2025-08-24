using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.Problems
{
    public class ProblemComment : ValueObject
    {
        public long ProblemId { get; private set; }
        public long UserId { get; private set; }
        public string Text { get; private set; }
        public DateTime TimeSent { get; private set; }

        [JsonConstructor]
        public ProblemComment(long problemId, long userId, string text, DateTime timeSent)
        {
            ProblemId = problemId;
            UserId = userId;
            Text = text;
            TimeSent = timeSent;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProblemId;
            yield return UserId;
            yield return Text;
            yield return TimeSent;
        }

    }
}
