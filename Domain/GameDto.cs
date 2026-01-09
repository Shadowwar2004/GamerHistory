namespace Domain;

// DTO pour la création d'un jeu (POST)
public class GameCreateDto
{
    public string Nom { get; set; } = string.Empty;
    
    // Correspond au "TempsEstimer" de la base de données
    public int TempsEstimer { get; set; } 
    
    // L'ID du support sur lequel le jeu tourne (ex: 1 pour PC)
    public int SupportId { get; set; }
}

// DTO pour la lecture d'un jeu (GET)
public class GameReadDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public int TempsEstimer { get; set; }
    public int SupportId { get; set; }
    
    // Optionnel : Ajoute le nom du support pour faciliter l'affichage côté Frontend
    public string? SupportNom { get; set; }
}