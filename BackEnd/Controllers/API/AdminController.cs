using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

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
    
    [Authorize(Roles = "admin, filmstudio")]
    [HttpPost("films/rent")]
    public async Task<IActionResult> RentAMovie([FromQuery] int id, [FromQuery] int studioId)
    {
        if (User.IsInRole("filmstudio"))
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(claim, out var userId)) return Unauthorized();
            var CorrectStudio = await _movies.StudioIdCheck(userId, studioId);
            if(CorrectStudio == null)return NotFound("error: JhGvFs87241");
            if(CorrectStudio == false)return Unauthorized("Filmstudio id mismath");
        }

        var result = await _movies.RentAMovie(id, studioId);
        if (result == null)return NotFound("error: REQnot404444");
        var (studio, films, error) = result.Value;
        if(error == "studio not found")return NotFound("Studio not found");
        if(error == "movie not found")return Conflict("movie not found");
        if(error == "error: jhgANB99643")return Conflict("error: jhgANB99643");
        if(error == "Studio already has this movie")return StatusCode(403, "Studio already has this movie");

        return Ok(studio);
    }
    [Authorize(Roles = "admin, filmstudio")]
    [HttpPost("films/return")]
    public async Task<IActionResult> ReturnAMovie([FromQuery] int id, [FromQuery] int studioId)
    {
        if (User.IsInRole("filmstudio"))
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(claim, out var userId)) return Unauthorized();
            var CorrectStudio = await _movies.StudioIdCheck(userId, studioId);
            if(CorrectStudio == null)return NotFound("error: JhGvFs87241");
            if(CorrectStudio == false)return Unauthorized("Filmstudio id mismath");
        }

        var result = await _movies.ReturnAMovie(id, studioId);
        if (result == null)return NotFound("error: REQnot404444");
        var (studio, films, error) = result.Value;
        if(error == "studio not found")return NotFound("Studio not found");
        if(error == "movie not found")return Conflict("movie not found");
        if(error == "error: jhgANB99643")return Conflict("error: jhgANB99643");
        if(error == "Studio does not have this movie")
            return StatusCode(403, "Studio does not have this movie");

        return Ok(studio);
    }

    [Authorize(Roles = "filmstudio")]
    [HttpGet("mystudio/rentals")]
    public async Task<IActionResult> GetRentedMovies()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!int.TryParse(claim, out var userId))return Unauthorized();
        var result = await _movies.GetRentedMovies(userId);
        if(result == null)return NotFound("couldnt find movies or movie");
        var (movie, error) = result.Value;
        if(error == "User or filmstudio not find") 
            return NotFound("User or filmstudio not find");
        if(error == "no studio found") 
            return NotFound("no studio found");
        if(error == "could not find specific movie")
            return NotFound("could not find specific movie");

        return Ok(movie);
    }
}