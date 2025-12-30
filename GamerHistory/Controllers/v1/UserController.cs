using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GamerHistory.Controllers.v1;

[ApiController]
[Route("/api/v1/users")]
public class UserController(IUserRepository userRepository): ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<User>> GetAll()
    {
        return Ok(userRepository.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetById(int id)
    {
        var user = userRepository.GetById(id);
        if (user != null)
        {
            return Ok(user);
        }
        return NotFound();
    }
}