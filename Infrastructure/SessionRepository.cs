using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SessionRepository(UserContext context) : ISessionRepository
{
    public Session Save(Session session)
    {
        // Si l'ID est 0, c'est un ajout. Sinon, EF Core gère la modification (si l'objet est suivi).
        if (session.Id == 0)
        {
            context.Sessions.Add(session);
        }
        else
        {
            context.Sessions.Update(session);
        }
        context.SaveChanges();
        return session;
    }

    public List<Session> GetByUser(int userId)
    {
        // On inclut le Game pour avoir son Nom et TempsEstimer (requis pour le calcul des 80%)
        return context.Sessions
            .Include(s => s.Game)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.DateRecord) // Plus récent en premier
            .ToList();
    }

    public Session? GetById(int id)
    {
        return context.Sessions.Include(s => s.Game).FirstOrDefault(s => s.Id == id);
    }

    public void Delete(int id)
    {
        var session = context.Sessions.Find(id);
        if (session != null)
        {
            context.Sessions.Remove(session);
            context.SaveChanges();
        }
    }
}