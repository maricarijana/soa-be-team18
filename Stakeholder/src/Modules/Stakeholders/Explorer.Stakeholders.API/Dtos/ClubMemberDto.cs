using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class ClubMemberDto
    {
        public long Id { get; set; }
        public string CurrentImage { get; set; }
        public string QuizImage { get; set; }
        public long UserId {  get; set; }
        public string ImageBase64 { get; set; }
    }
}
