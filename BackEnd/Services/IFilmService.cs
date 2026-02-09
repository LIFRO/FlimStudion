using BackEnd.Models;

namespace BackEnd.Services;

public interface IFilmService
{
    Task<Film?> CreateMovie(CreateFilm request);
    
    Task<FilmStudio[]?> GetStudios();

    Task<FilmStudio?> GetStudioById(string? userId, string? role, int studioId);

    Task<Film[]?> GetAllFilms();

    Task<Film?> GetFilmById(int id);

    Task<Film?> UpdateMovie(int dtoId, Film dto);

    Task<(Film film, string error)?> ChangeMovieCopyAmount(int id, int amountWanted);
}