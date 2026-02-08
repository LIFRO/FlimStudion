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
    public async Task<IActionResult> GetMovieStudioById([FromRoute] int id)
    {
        var userIdClaimId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoleClaim = User?.FindFirstValue(ClaimTypes.Role);

        var studio = await _movies.GetStudioById(userIdClaimId, userRoleClaim, id);
        if(studio == null)return NotFound("studio dosnt exist");

        return Ok(studio);
    }

    [HttpGet("films")]
    public async Task<IActionResult> GetAllFilms()
    {
        var films = await _movies.GetAllFilms();
        if(films == null) return NotFound("no films exist");

        if(User.Identity != null && User.Identity.IsAuthenticated == true) return Ok(films);
        
        var dto = films.Select(f => new
        {
            f.FilmId,
            f.Title,
            f.Genre
        });

        return Ok(dto);
    }

    [HttpGet("films/{id}")]
    public async Task<IActionResult> GetFilm([FromRoute] int id)
    {
        var film = await _movies.GetFilmById(id);
        if(film == null)return NotFound("That movie dosnt exist");

        if(User.Identity != null && User.Identity.IsAuthenticated == true) return Ok(film);

        var dto = new
        {
            FilmId = film.FilmId,
            Title = film.Title,
            Genre = film.Genre
        };

        return Ok(dto);
    }
}
