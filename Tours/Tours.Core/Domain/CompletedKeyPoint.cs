using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tours.Core.Domain
{
    public class CompletedKeyPoint : ValueObject
    {

        public long KeyPointId { get; init; }

        public DateTime CompletedTime { get; init; }

        [JsonConstructor]
        public CompletedKeyPoint(long keyPointId, DateTime completedTime)
        {
            KeyPointId = keyPointId;
            CompletedTime = completedTime;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return KeyPointId;
            yield return CompletedTime;
        }
    }
}
