namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Pseudo { get; set; }
    public string Email { get; set; }
    // Stockage binaire (60 bytes pour BCrypt) [cite: 20]
    public byte[] Password { get; set; } = Array.Empty<byte>();
    public string Role { get; set; }
}