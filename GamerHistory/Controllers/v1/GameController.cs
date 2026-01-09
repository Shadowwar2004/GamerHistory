using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GamerHistory.Controllers.v1;

[ApiController]
[Route("games")] // Route définie dans le PDF
public class GameController(IGameRepository gameRepository) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<GameReadDto> Create([FromBody] GameCreateDto input)
    {
        // Validation simple : vérifie si le jeu existe déjà sur ce support
        if (gameRepository.Exists(input.Nom, input.SupportId))
        {
            return Conflict($"Le jeu '{input.Nom}' existe déjà sur ce support.");
        }

        // Mapping manuel DTO vers Entité
        var gameToSave = new Game
        {
            Nom = input.Nom,
            TempsEstimer = input.TempsEstimer,
            SupportId = input.SupportId
        };

        var savedGame = gameRepository.Save(gameToSave);

        // Mapping manuel Entité vers DTO de lecture
        var gameRead = new GameReadDto
        {
            Id = savedGame.Id,
            Nom = savedGame.Nom,
            TempsEstimer = savedGame.TempsEstimer,
            SupportId = savedGame.SupportId,
            // Astuce : Si savedGame.Support est null ici, le nom sera null, ce n'est pas grave pour la création
            SupportNom = savedGame.Support?.Nom 
        };

        // Retourne 201 Created
        return CreatedAtAction(nameof(Create), new { id = gameRead.Id }, gameRead);
    }
}