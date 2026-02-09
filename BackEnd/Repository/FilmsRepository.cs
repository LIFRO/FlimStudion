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

    public async Task<List<FilmCopy>> SaveCopies(List<FilmCopy> request)
    {
        _db.FilmCopies.AddRange(request);
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

    public async Task<Film?> RemoveFilmCopies(int id, int amount)
    {
        var copies = await _db.FilmCopies
            .Where(c => c.FilmId == id && c.LoanedByFilmStudioId == null)
            .OrderBy(c => c.FilmCopyId)
            .Take(amount)
            .ToListAsync();

            _db.FilmCopies.RemoveRange(copies);
            await _db.SaveChangesAsync();
            return await _db.Films
                .Include(f => f.FilmCopies)
                .FirstOrDefaultAsync(f => f.FilmId == id);
    }

    public async Task<FilmCopy?> RentAMovie(int filmId, int studioId)
    {
        var studio = await _db.FilmStudios
        .Include(s => s.RentedFilms)
        .FirstOrDefaultAsync(s => s.FilmStudioId == studioId);
        if (studio == null) return null;

        var filmCopy = await _db.FilmCopies
        .OrderBy(c => c.FilmCopyId)
        .FirstOrDefaultAsync(c => c.FilmId == filmId && c.LoanedByFilmStudioId == null);
        if(filmCopy == null)return null;

        filmCopy.LoanedByFilmStudioId = studioId;
        studio.RentedFilms.Add(filmCopy);
        await _db.SaveChangesAsync();
        return filmCopy;
    }

        public async Task<FilmCopy?> ReturnAMovie(int filmId, int studioId)
    {
        var studio = await _db.FilmStudios
            .Include(s => s.RentedFilms)
            .FirstOrDefaultAsync(s => s.FilmStudioId == studioId);
            if(studio == null)return null;

        var filmCopy = await _db.FilmCopies
        .OrderBy(c => c.FilmCopyId)
        .FirstOrDefaultAsync(c => c.FilmId == filmId && c.LoanedByFilmStudioId == studioId);
        if(filmCopy == null)return null;
        
        filmCopy.LoanedByFilmStudioId = null;
        studio.RentedFilms.Remove(filmCopy);
        await _db.SaveChangesAsync();
        return filmCopy;
    }

    public Task<bool> StudioHasLoanAsync(int filmId, int studioId) =>
    _db.FilmCopies.AsNoTracking()
        .AnyAsync(c => c.FilmId == filmId && c.LoanedByFilmStudioId == studioId);
}