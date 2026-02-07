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
        
        var dto = new User
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Role = user.Role
        };

        if(user.Role == "filmstudio"){
            dto.FilmStudio = user.FilmStudio;
        };

        return Ok(new{ user =dto, token });
    }
}