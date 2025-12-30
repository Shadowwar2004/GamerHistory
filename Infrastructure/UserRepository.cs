using System.Text;
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

    public User Save(User user)
    {
        if (user.Password == null || user.Password.Length == 0)
        {
            throw new ArgumentException("le mot de passe ne doit pas avoir d'espace ou etre vide");
        }

        string plainPassword = Encoding.UTF8.GetString(user.Password);
        string hash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        user.Password = Encoding.UTF8.GetBytes(hash);
        userContext.Users.Add(user);
        userContext.SaveChanges();
        return new User
        {
            Id = user.Id,
            Pseudo = user.Pseudo,
            Email = user.Email,
            Role = user.Role,
        };
    }
}