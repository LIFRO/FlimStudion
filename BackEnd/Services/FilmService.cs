using System.Diagnostics;
using System.Runtime.CompilerServices;
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

    private async Task<bool> CreateCopies(int filmId, int aomunt)
    {
        var stopwatch = Stopwatch.StartNew();
        for(int i = 0; i < aomunt; i++)
        {
            var copy = new FilmCopy
            {
                FilmId = filmId
            };
            var copySaveSucess = await _movies.SaveCopy(copy);
            if(copySaveSucess == null) return false; 
        }
        stopwatch.Stop();
        System.Console.WriteLine(stopwatch.Elapsed);

        stopwatch = Stopwatch.StartNew();
        var copies = new List<FilmCopy>();
        for(int i = 0; i < aomunt; i++)
        {
            copies.Add(new FilmCopy{ FilmId = filmId});

        }
            var copiesSaveSucess = await _movies.SaveCopies(copies);
            if(copiesSaveSucess == null) return false; 
        stopwatch.Stop();
        System.Console.WriteLine(stopwatch.Elapsed);

        return true;
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

        await CreateCopies(savedMovie.FilmId, request.Copies);
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

    public async Task<Film?> UpdateMovie(int dtoId, Film dto)
    {
        return await _movies.ChangeMovie(dto, dtoId);
    }

    public async Task<(Film film, string error)?> ChangeMovieCopyAmount(int id, int amountWanted)
    {
        var film = await _movies.GetFilmWithId(id);
        if(film == null) return null;
        int existingCopies = film.FilmCopies.Count();

        if(existingCopies > amountWanted)
        {
            var removeCopiesAmount = existingCopies - amountWanted;
            var available = film.FilmCopies.Count(c => string.IsNullOrEmpty(c.LoanedByFilmStudioId));
            if(available < amountWanted) return new(film, "error");
            var movie = await _movies.RemoveFilmCopies(film.FilmId, removeCopiesAmount);
            if(movie == null) return null;
            return new(movie, "");
        }

        var copyCreationSuccess = await CreateCopies(id, amountWanted);

        if(copyCreationSuccess == false) return new(film, "Code:zoBzjad#55773");

        return new(film, "");
    }
}