using BackEnd.Dto;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.API;

[Route("/api/filmstudio/")]
[ApiController]
public class FilmStudioController  : ControllerBase
{
private readonly IUserService _user;

public FilmStudioController(IUserService user)
    {
        _user = user;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
    {
        if(request.City is null) return BadRequest();
        if(request.StudioName is null) return BadRequest();

        var newUser = await _user.RegisterAsync(request, "filmstudio");
        if(newUser is null) return BadRequest();
        if(newUser.FilmStudio is null) return BadRequest();
    var dto = new FilmStudioUserDto
    {
    FilmStudioId = newUser.FilmStudio.FilmStudioId,
    StudioName = newUser.FilmStudio.StudioName ?? "",
    City = newUser.FilmStudio.City ?? "",
    RentedFilms = newUser.FilmStudio.RentedFilms
    };

        return Ok(dto);
    }
}