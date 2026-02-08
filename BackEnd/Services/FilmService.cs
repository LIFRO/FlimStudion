using BackEnd.Dto;
using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _movies;
    private readonly IUserRepository _users;

    public FilmService(IFilmRepository movies, IUserRepository users)
    {
        _movies = movies;
        _users = users;
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

    public async Task<FilmStudio?> GetStudioById(string? userIdClaim, string? role, int studioId)
    {
        var studio = await _movies.GetStudioWithId(studioId);
        if(studio == null) return null;
        studio = new FilmStudio
        {
            FilmStudioId = studio.FilmStudioId,
            StudioName = studio.StudioName
        };

        if(role == "admin")
        {
            var adminStudio = await _movies.GetStudioWithId(studioId);

            if (adminStudio == null) return null;
            return adminStudio;
        }

        if(!int.TryParse(userIdClaim, out var userId))
        {
            var user = await _users.FindUserWithInt(userId);
            
            if(user == null || user.FilmStudio == null) return studio;
            if(user.FilmStudio.FilmStudioId == studioId) return await _movies.GetStudioWithId(user.FilmStudio.FilmStudioId);
            return studio;
        }

        return studio;
    }

    public async Task<Film[]?> GetAllFilms()
    {
        var films = await _movies.GetAllFilms();

        return films;
    }


    public async Task<Film?> GetFilmById(int id)
    {
        return await _movies.GetFilmWithId(id);

    }
}