using Explorer.Stakeholders.Core.Domain;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Authentication
{
    public class RemoveMemberTest
    {
            [Fact]
            public void RemoveMember_RemovesUserId_FromList()
            {
                // Arrange - Kreiramo klub sa 3 člana
                var club = new Club("Planinarski klub", "Opis kluba", null, 1, new List<Club.ClubTags> { Club.ClubTags.Adventure, Club.ClubTags.Wildlife });
                club.UserIds.AddRange(new List<long> { 101, 102, 103 });  // Dodavanje 3 userId-a

                var initialCount = club.UserIds.Count;  // Pamti se početni broj članova
                var userIdToRemove = 102;  // Id koji želimo da uklonimo

                // Act - Pozivamo metodu za brisanje člana
                club.UserIds.Remove(userIdToRemove);

                // Assert - Proveravamo da je član uspešno obrisan
                club.UserIds.Count.ShouldBe(initialCount - 1);  // Trebalo bi da bude 2
                club.UserIds.ShouldNotContain(userIdToRemove);  // Id ne bi trebao da postoji u listi
            }
        


    }
}
