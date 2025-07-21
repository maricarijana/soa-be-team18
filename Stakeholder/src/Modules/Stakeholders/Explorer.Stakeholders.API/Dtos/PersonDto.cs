

namespace Explorer.Stakeholders.API.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string? Biography { get; set; }
        public string? Motto { get; set; }
        public List<int> Equipment { get; set; }
        public string? ImageBase64 { get; set; }
        public decimal Wallet {  get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
    }
}
