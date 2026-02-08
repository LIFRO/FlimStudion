using BackEnd.Models;

namespace BackEnd.Services;

public interface IFilmService
{
    Task<Film?> CreateMovie(CreateFilm request);
    
    Task<FilmStudio[]?> GetStudios();

    Task<FilmStudio?> GetStudio(string? userId, string? role, int studioId);
}