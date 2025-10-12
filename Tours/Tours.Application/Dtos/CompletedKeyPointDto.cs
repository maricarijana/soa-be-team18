using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tours.Application.Dtos
{
    public class CompletedKeyPointDto
    {
        public long KeyPointId { get; set; }
        public DateTime CompletedTime { get; set; }

        public CompletedKeyPointDto() { }

        [JsonConstructor]
        public CompletedKeyPointDto(long keyPointId, DateTime completedTime)
        {
            KeyPointId = keyPointId;
            CompletedTime = completedTime;
        }
    }
}
