using System.Security.Claims;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[Route("/api")]
[ApiController]
public class PublicController : ControllerBase
{
    private readonly IFilmService _movies;

    public PublicController(IFilmService movies)
    {
        _movies = movies;
    }

    [HttpGet("filmstudios")]
    public async Task<IActionResult> GetMovieStudios()
    {
        var studios = await _movies.GetStudios();
        if(studios == null) return NotFound("No studios found");

        if(User.IsInRole("admin")){
            return Ok(studios);
        }

        var dto = studios.Select(s => new
        {
            s.FilmStudioId,
            s.StudioName
        });

        return Ok(dto);
    }

    [HttpGet("filmstudios/{id}")]
    public async Task<IActionResult> GetMovieStudio([FromRoute] int id)
    {
        var userIdClaimId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoleClaim = User?.FindFirstValue(ClaimTypes.Role);

        var studio = await _movies.GetStudio(userIdClaimId, userRoleClaim, id);
        if(studio == null)return NotFound("studio dosnt exist");

        return Ok(studio);
    }
}
