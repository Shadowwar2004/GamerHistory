using Domain;

namespace Infrastructure;

public interface ISessionRepository
{
    Session Save(Session session);
    List<Session> GetByUser(int userId);
    Session? GetById(int id);
    void Delete(int id);
}