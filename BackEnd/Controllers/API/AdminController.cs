using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Services;

namespace BackEnd.Controllers;

[Route("/api/")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IFilmService _movies;

    public AdminController(IFilmService movies)
    {
        _movies = movies;
    }
    [Authorize(Roles = "admin")]
    [HttpPost("films")]
    public async Task<IActionResult> CreateMovie(CreateFilm request)
    {
        var movie = await _movies.CreateMovie(request);
        if (movie == null) return BadRequest();

        var dto = new Film
        {
            Title = movie.Title,
            Genre = movie.Genre,
            FilmCopies = movie.FilmCopies
        };
        return Ok(dto);
    }
}