using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ClubMember:Entity
    {
        public string CurrentImage { get; private set; }
        public string QuizImage { get; private set; }
        public long UserId {  get; private set; }
        public ClubMember() { }

      public ClubMember(string currentImage, string quizImage, long userId)
        {
            CurrentImage = currentImage;
            QuizImage = quizImage;
            UserId = userId;
            Validate();
        }

        public void Validate()
        {
        }

    }
}
