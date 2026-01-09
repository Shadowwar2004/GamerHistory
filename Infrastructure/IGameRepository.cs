using Domain;

namespace Infrastructure;

public interface IGameRepository
{
    Game Save(Game game);
    List<Game> GetBySupport(int supportId);
    
    // Utile pour vérifier si un jeu existe déjà lors de la création
    bool Exists(string name, int supportId);
}