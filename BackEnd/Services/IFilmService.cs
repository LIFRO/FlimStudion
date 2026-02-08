using BackEnd.Models;

namespace BackEnd.Services;

public interface IFilmService
{
    Task<Film?> CreateMovie(CreateFilm request);
    
    Task<FilmStudio[]?> GetStudios();
}