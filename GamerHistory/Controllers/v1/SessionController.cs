using System.Text;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GamerHistory.Controllers.v1;

[ApiController]
[Route("/session")] // Route requise  
public class SessionController(IUserRepository userRepository, JwtService jwtService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult Login([FromBody] UserLoginDto loginDto)
    {
        // 1. Récupérer l'utilisateur
        var user = userRepository.GetByEmail(loginDto.Email);
        if (user is null) return Unauthorized("Utilisateur inconnu.");

        // 2. Vérifier le mot de passe
        string storedHash = Encoding.UTF8.GetString(user.Password);
        
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, storedHash);
        if (!isPasswordValid) return Unauthorized("Mot de passe incorrect.");

        // 3. Générer le JWT
        var token = jwtService.GenerateToken(user);

        // 4. Stocker dans un Cookie HttpOnly 
        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Mettre false si tu testes en HTTP local sans HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddMinutes(60)
        });

        // Dans Login return :
        return Ok(new { message = "Authentifié", userId = user.Id }); 
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Logout()
    {
        
        if (!Request.Cookies.ContainsKey("jwt"))
        {
            return NotFound(); 
        }
        Response.Cookies.Delete("jwt");
        return NoContent(); 
    }
}