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

        return Ok(movie);
    }

    [Authorize(Roles = "admin")]
    [HttpPatch("Films/{id}")]
    public async Task<IActionResult> ChangeMovieDetails([FromRoute] int id, [FromBody] Film film)
    {
        var changedMovie = await _movies.UpdateMovie(id, film);
        if(changedMovie == null)return NotFound("that movie dosnt exist");

        changedMovie.FilmCopies = new List<FilmCopy>{};
        return Ok(changedMovie);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("Films/{id}")]
    public async Task<IActionResult> ChangeMovieCopyAmount ([FromRoute]int id, [FromBody] int copyAmount)
    {
        var result = await _movies.ChangeMovieCopyAmount(id, copyAmount);
        var (movie, error) = result.Value;
        if(movie == null) return NotFound("movie not found"); 
        if(error == "error") return Conflict("not enough non loaned copies to reduce amount");

        return Ok(movie);
    }
}