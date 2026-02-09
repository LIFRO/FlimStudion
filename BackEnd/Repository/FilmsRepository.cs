using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository;

public class FilmRepository : IFilmRepository
{
    private readonly AppDbContext _db;

    public FilmRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> DoesMovieExist(string request) => 
        await _db.Films.AsNoTracking().AnyAsync(f => f.Title == request);

    public async Task<Film> SaveMovie(Film request)
    {
        _db.Films.Add(request);
        await _db.SaveChangesAsync();

        return request;
    }

    public async Task<FilmCopy> SaveCopy(FilmCopy request)
    {
        _db.FilmCopies.Add(request);
        await _db.SaveChangesAsync();

        return request;
    }

    public async Task<FilmStudio[]?> GetStudios()
    {
        var studios = await _db.FilmStudios
            .AsNoTracking()
            .Include(l => l.RentedFilms)
            .ToArrayAsync();

        return studios;
    }

    public async Task<FilmStudio?> GetStudioWithId(int Id)
    {
        return await _db.FilmStudios
            .Include(f => f.RentedFilms)
            .FirstOrDefaultAsync(f => f.FilmStudioId == Id);
    }

    public async Task<Film[]?> GetAllFilms()
    {
        return await _db.Films
            .AsNoTracking()
            .Include(f => f.FilmCopies)
            .ToArrayAsync();
    }

    public async Task<Film?> GetFilmWithId(int id)
    {
        return await _db.Films
        .Include(f => f.FilmCopies)
        .FirstOrDefaultAsync(f => f.FilmId == id);
    }

    public async Task<Film?> ChangeMovie(Film dto, int dtoId)
    {
        var film = await _db.Films.Include(f => f.FilmCopies)
                                    .FirstOrDefaultAsync(f => f.FilmId == dtoId);
        if(film == null) return null;

        film.Title = dto.Title;
        film.Genre = dto.Genre;
        await _db.SaveChangesAsync();

        return film;
    }
}