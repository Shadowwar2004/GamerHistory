using Domain;

namespace Infrastructure;

public class UserRepository (UserContext userContext) : IUserRepository
{
    public List<User> GetAll()
    {
        return userContext.Users.ToList();
    }

    public User? GetById(int id)
    {
        return userContext.Users.FirstOrDefault(u=>u.Id==id);
    }

    
}