namespace Domain;

public class Session
{
    public int Id { get; set; }
    public int Temps { get; set; } // En minutes
    public DateTime DateRecord { get; set; }
    
    // Clés étrangères
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
}