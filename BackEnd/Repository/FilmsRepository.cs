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
}