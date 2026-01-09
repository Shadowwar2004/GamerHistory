using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GameRepository(UserContext context) : IGameRepository
{
    public Game Save(Game game)
    {
        context.Games.Add(game);
        context.SaveChanges();
        return game;
    }

    public List<Game> GetBySupport(int supportId)
    {
        // On récupère les jeux d'un support spécifique
        return context.Games
            .Include(g => g.Support) // Optionnel : si on veut renvoyer le nom du support avec
            .Where(g => g.SupportId == supportId)
            .ToList();
    }

    public bool Exists(string name, int supportId)
    {
        return context.Games.Any(g => g.Nom == name && g.SupportId == supportId);
    }
}