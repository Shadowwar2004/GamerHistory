using Domain;
namespace Infrastructure;

public interface IUserRepository
{
    List<User> GetAll();
    User? GetById(int id);
    
}