namespace Domain;

public class SessionCreateDto
{
    public int GameId { get; set; }
    public int Temps { get; set; } // Temps joué en minutes
    public DateTime DateRecord { get; set; }
}

public class SessionReadDto
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string GameNom { get; set; } = string.Empty;
    public int GameTempsEstimer { get; set; } // Nécessaire pour le calcul frontend
    public int Temps { get; set; }
    public DateTime DateRecord { get; set; }
}