using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _movies;

    public FilmService(IFilmRepository movies)
    {
        _movies = movies;
    }

    public async Task<Film?> CreateMovie(CreateFilm request)
    {
        if(await _movies.DoesMovieExist(request.Title)) return null;
        var movie = new Film
        {
            Title = request.Title,
            Genre = request.Genre
        };

        var savedMovie = await _movies.SaveMovie(movie);

        for(int i = 0; i < request.Copies; i++)
        {
            var copy = new FilmCopy
            {
                FilmId = savedMovie.FilmId
            };
            await _movies.SaveCopy(copy);
        }

        return movie;
    }

    public async Task<FilmStudio[]?> GetStudios()
    {
        var studios = await _movies.GetStudios();

        if(studios == null)
        {
            return null;
        }

        return studios;
    }
}