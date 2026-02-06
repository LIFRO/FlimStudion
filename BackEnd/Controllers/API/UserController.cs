using BackEnd.Dto;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.API;

[Route("/api/user/")]
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
        if(newUser is null) return BadRequest();
    var dto = new AdminUserDto
    {
    UserId = newUser.UserId,
    UserName = newUser.UserName,
    Role = newUser.Role
    };

        return Ok(dto);
    }
}