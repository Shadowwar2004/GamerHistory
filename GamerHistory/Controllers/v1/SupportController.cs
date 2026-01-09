using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GamerHistory.Controllers.v1;

[ApiController]
[Route("supports")]
public class SupportController(ISupportRepository supportRepository, IGameRepository gameRepository) : ControllerBase
{
    // Indispensable pour la liste déroulante du Frontend
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<SupportReadDto>> GetAll()
    {
        var supports = supportRepository.GetAll();
        
        var result = supports.Select(s => new SupportReadDto
        {
            Id = s.Id,
            Nom = s.Nom
        }).ToList();

        return Ok(result);
    }

    // Route demandée : /supports/{id}/games
    [HttpGet("{id:int}/games")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<GameReadDto>> GetGamesBySupport(int id)
    {
        var games = gameRepository.GetBySupport(id);

        var result = games.Select(g => new GameReadDto
        {
            Id = g.Id,
            Nom = g.Nom,
            TempsEstimer = g.TempsEstimer,
            SupportId = g.SupportId,
            SupportNom = g.Support?.Nom
        }).ToList();

        return Ok(result);
    }
}