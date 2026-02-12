using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Dtos.Users;
using backend.Models;
using backend.Services;
namespace backend.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UserController : ControllerBase
{
    private readonly UserService _users;

    public UserController(UserService users)
    {
        _users = users;
    }

    [HttpPost("")]
    public async Task<ActionResult<RegisterUser>> Register(UserCredentials request)
    {
        var response = await _users.RegisterAsync(request);
        return Created(string.Empty, response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUser>> Login(UserCredentials request)
    {
        var response = await _users.LoginAsync(request);
        return Ok(response);
    }

    // endpoint de prueba para listar usuarios, se puede eliminar después
    [Authorize]
    [HttpGet("")]
    public async Task<ActionResult<List<Users>>> GetAllUsers()
    {
        var users = await _users.GetAllAsync();
        return Ok(users);
    }
}