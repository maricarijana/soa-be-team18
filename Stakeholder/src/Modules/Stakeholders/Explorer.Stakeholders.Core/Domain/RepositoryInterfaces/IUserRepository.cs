namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    bool Exists(string username);
    User? GetActiveByName(string username);
    User Create(User user);   //kt1
    long GetPersonId(long userId);
    List<User> GetActiveUsers();      //kt1? ili all 
    User? GetById(long userId);
    bool IsAuthor(long userId);
}