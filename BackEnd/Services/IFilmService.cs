using BackEnd.Models;

namespace BackEnd.Services;

public interface IFilmService
{
    public Task<Film?> CreateMovie(CreateFilm request);
}