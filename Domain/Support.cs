namespace Domain;

public class Support
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;

    // Propriété de navigation : Un support peut avoir plusieurs jeux
    public List<Game> Games { get; set; } = new();
}