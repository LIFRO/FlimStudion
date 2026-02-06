using BackEnd.Data;
using BackEnd.Models;

namespace BackEnd.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> AddFilmStudioAsync(FilmStudio filmStudio, User user)
    {
        _db.FilmStudios.Add(filmStudio);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

        public async Task<User> AddUserAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}