using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ProblemDTO
    {
        public long? Id { get; set; }
        public long UserId { get; init; }
        public long TourId { get; init; }
        //public Category Catgory { get; set; }
        public string Category { get; init; }
        public string Description { get; init; }
        public int Priority { get; set; }
        public DateTime Time { get; set; }
        public bool IsActive { get; set; }
       
        public List<ProblemCommentDto> Comments { get; set; }
        public long Deadline { get; set; }
    }

}
