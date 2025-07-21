using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ProblemCommentDto
    {
        public long ProblemId { get;  set; }
        public long UserId { get;  set; }
        public string Text { get;  set; }
        public DateTime TimeSent { get;  set; }
    }
}
