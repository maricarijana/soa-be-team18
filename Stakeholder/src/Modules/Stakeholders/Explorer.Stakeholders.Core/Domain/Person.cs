using Explorer.BuildingBlocks.Core.Domain;
using System.Net.Mail;
using System.Net.Security;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Core.Domain;

public class Person : Entity
{
    public long UserId { get; init; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; init; }
    public string? ImageUrl {  get; private set; }
    public string? Biography { get; private set; }
    public string? Motto { get; private set; }
    public List<int>? Equipment {  get; private set; }
    public decimal? Wallet { get; private set; }
    public int XP { get; private set; }
    public int Level { get; private set; }

    public Person(long userId, string name, string surname, string email)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Email = email;
        XP = 0;
        Level = 1;
        Validate();
        
    }
    public Person(long userId, string name, string surname, string email, string imageUrl, string biography, string motto, decimal wallet)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Email = email;
        ImageUrl = imageUrl;
        Biography = biography;
        Motto = motto;
        Wallet = wallet;
        XP = 0;
        Level = 1;
        Validate();
    }

    public void AddXP(int amount)
    {
        if (amount < 0) throw new ArgumentException("XP can not be negative!");
        XP += amount;
        CheckLevelUp();
    }

    public void CheckLevelUp()
    {
        const int xpPerLevel = 100;
        while (XP >= xpPerLevel * Level)
        {
            Level++;
        }
    }
    
    private void Validate()
    {
        if (UserId == 0) throw new ArgumentException("Invalid UserId");
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
        if (string.IsNullOrWhiteSpace(Surname)) throw new ArgumentException("Invalid Surname");
        if (!MailAddress.TryCreate(Email, out _)) throw new ArgumentException("Invalid Email");
    }
}