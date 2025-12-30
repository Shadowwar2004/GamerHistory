namespace Domain;

public class UserCreateDto
{
    public string Pseudo { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class UserReadDto
{
    public int Id { get; set; }
    public string Pseudo { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}

public class UserLoginDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class SessionResponseDto
{
    public string token { get; set; } = default!;
    
}