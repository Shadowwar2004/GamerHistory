using Domain;

namespace Infrastructure;

public interface ISupportRepository
{
    List<Support> GetAll();
    Support? GetById(int id);
}