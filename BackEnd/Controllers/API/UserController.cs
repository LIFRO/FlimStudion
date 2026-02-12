using BackEnd.Dto;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.API;

[Route("/api/users/")]
[ApiController]
public class UserController  : ControllerBase
{
private readonly IUserService _user;

public UserController(IUserService user)
    {
        _user = user;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
    {
        if(request.IsAdmin == false) return StatusCode(418, "brewing tea");
        var newUser = await _user.RegisterAsync(request, "admin");
        if(newUser is null) return Conflict("Username already exists");
    var dto = new AdminUserDto
    {
    UserId = newUser.UserId,
    UserName = newUser.UserName,
    Role = newUser.Role
    };

        return Ok(dto);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] UserLoginDto request)
    {
        var result = await _user.LoginAsync(request);
        if(result == null) return Unauthorized("Invalid credentials");
        var (user, token) = result.Value;

        object dto;

        if(user.Role == "admin"){
            dto = new AdminUserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Role = user.Role
            };
        }
        else
        {
            dto = new FilmStudioDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Role = user.Role,
                filmStudio = user.FilmStudio
            };
        }

        return Ok(new{ dto, token });
    }
}