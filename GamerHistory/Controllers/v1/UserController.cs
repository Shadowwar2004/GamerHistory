using System.Text;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GamerHistory.Controllers.v1;

[ApiController]
[Route("users")] // Route simplifiée pour correspondre au PDF et au Frontend
public class UserController(IUserRepository userRepository, ISessionRepository sessionRepository, IGameRepository gameRepository) : ControllerBase
{
    // --- GESTION UTILISATEURS ---

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<UserReadDto>> GetAll()
    {
        var users = userRepository.GetAll();
        var result = users.Select(u => new UserReadDto
        {
            Id = u.Id,
            Pseudo = u.Pseudo,
            Email = u.Email,
            Role = u.Role
        }).ToList();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserReadDto> GetById(int id)
    {
        var user = userRepository.GetById(id);
        if (user is null) return NotFound();

        return Ok(new UserReadDto
        {
            Id = user.Id,
            Pseudo = user.Pseudo,
            Email = user.Email,
            Role = user.Role
        });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<UserReadDto> Save([FromBody] UserCreateDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Password))
            return BadRequest("Le mot de passe ne peut pas être vide.");
        
        var user = new User
        {
            Pseudo = input.Pseudo,
            Email = input.Email,
            Password = Encoding.UTF8.GetBytes(input.Password),
            Role = "User"
        };
        
        var saved = userRepository.Save(user);
        var read = new UserReadDto
        {
            Id = saved.Id,
            Pseudo = saved.Pseudo,
            Email = saved.Email,
            Role = saved.Role
        };
        
        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    // --- GESTION DES SESSIONS (HISTORIQUE) ---

    // 1. AJOUTER UNE SESSION
    [HttpPost("{id:int}/history/records")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SessionReadDto> AddSession(int id, [FromBody] SessionCreateDto input)
    {
        var user = userRepository.GetById(id);
        if (user is null) return NotFound("Utilisateur introuvable.");

        var session = new Session
        {
            UserId = id,
            GameId = input.GameId,
            Temps = input.Temps,
            DateRecord = input.DateRecord
        };

        var savedSession = sessionRepository.Save(session);

        // Récupération "bricolée" du nom du jeu pour le retour direct (optionnel)
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, new SessionReadDto 
        { 
            Id = savedSession.Id, 
            Temps = savedSession.Temps, 
            DateRecord = savedSession.DateRecord,
            GameId = savedSession.GameId,
            GameNom = "Jeu ajouté (Rafraichir pour le nom)" 
        });
    }

    // 2. AFFICHER L'HISTORIQUE (GET)
    [HttpGet("{id:int}/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<SessionReadDto>> GetHistory(int id)
    {
        var sessions = sessionRepository.GetByUser(id);
        
        var result = sessions.Select(s => new SessionReadDto
        {
            Id = s.Id,
            GameId = s.GameId,
            GameNom = s.Game != null ? s.Game.Nom : "Inconnu",
            GameTempsEstimer = s.Game != null ? s.Game.TempsEstimer : 0,
            Temps = s.Temps,
            DateRecord = s.DateRecord
        }).ToList();

        return Ok(result);
    }

    // 3. MODIFIER UNE SESSION (PUT)
    [HttpPut("{id:int}/history/records/{sessionId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateSession(int id, int sessionId, [FromBody] SessionCreateDto input)
    {
        var session = sessionRepository.GetById(sessionId);
        
        if (session is null) return NotFound("Session introuvable.");
        if (session.UserId != id) return BadRequest("Cette session n'appartient pas à cet utilisateur.");

        session.Temps = input.Temps;
        session.DateRecord = input.DateRecord;
        // On ne change généralement pas le jeu d'une session, mais c'est possible :
        // session.GameId = input.GameId; 

        sessionRepository.Save(session);

        return NoContent();
    }

    // 4. SUPPRIMER UNE SESSION (DELETE)
    [HttpDelete("{id:int}/history/records/{sessionId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteSession(int id, int sessionId)
    {
        var session = sessionRepository.GetById(sessionId);
        
        if (session is null) return NotFound();
        if (session.UserId != id) return BadRequest("Interdit.");

        sessionRepository.Delete(sessionId);
        return NoContent();
    }
}