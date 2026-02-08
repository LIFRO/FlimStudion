using BackEnd.Models;

namespace BackEnd.Repository;

public interface IFilmRepository
{
    Task<bool> DoesMovieExist(string request);

    Task<Film> SaveMovie(Film request);
    
    Task<FilmCopy> SaveCopy(FilmCopy request);

    Task<FilmStudio[]?> GetStudios();

    Task<FilmStudio?> GetStudioWithId(int id);
}