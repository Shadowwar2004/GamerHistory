using System.Text;
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
    public ActionResult<UserReadDto> Save([FromBody]UserCreateDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Password))
            return BadRequest("pas de mot de passe vide ");
        
        string hash = BCrypt.Net.BCrypt.HashPassword(input.Password);
        byte[] hashbyte=Encoding.UTF8.GetBytes(hash);
        var user = new User
        {
            Pseudo = input.Pseudo,
            Email = input.Email,
            Password = hashbyte,
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
}