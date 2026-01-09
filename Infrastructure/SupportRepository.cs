using Domain;

namespace Infrastructure;

public class SupportRepository(UserContext context) : ISupportRepository
{
    public List<Support> GetAll()
    {
        return context.Supports.ToList();
    }

    public Support? GetById(int id)
    {
        return context.Supports.FirstOrDefault(s => s.Id == id);
    }
}