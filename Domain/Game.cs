namespace Domain;

public class Game
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    
    // Le temps estimé en minutes (INT dans le SQL)
    public int TempsEstimer { get; set; }

    // Clé étrangère vers Support
    public int SupportId { get; set; }
    
    // Propriété de navigation
    public Support Support { get; set; } = null!;
}