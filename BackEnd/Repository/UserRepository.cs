using BackEnd.Data;
using BackEnd.Dto;
using BackEnd.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> FindUser(UserLoginDto request)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == request.UserName);
    
        return user;
    }

    public async Task<User> AddFilmStudioAsync(FilmStudio filmStudio, User user)
    {
        _db.FilmStudios.Add(filmStudio);
        await _db.SaveChangesAsync();

        await AddUserAsync(user);
        return user;
    }

        public async Task<User> AddUserAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UserExists(string user) =>
        await _db.Users.AsNoTracking().AnyAsync(u => u.UserName == user);

}