using BackEnd.Models;

namespace BackEnd.Repository;

public interface IFilmRepository
{
    Task<bool> DoesMovieExist(string request);

    Task<Film> SaveMovie(Film request);
    
    Task<FilmCopy> SaveCopy(FilmCopy request);

    Task<List<FilmCopy>> SaveCopies(List<FilmCopy> request);

    Task<FilmStudio[]?> GetStudios();

    Task<FilmStudio?> GetStudioWithId(int id);

    Task<Film[]?> GetAllFilms();

    Task<Film?> GetFilmWithId(int id);

    Task<Film?> ChangeMovie(Film dto, int dtoId);

    Task<Film?> RemoveFilmCopies(int id, int amount);
}