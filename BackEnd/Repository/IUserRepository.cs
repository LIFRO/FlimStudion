using BackEnd.Models;

namespace BackEnd.Repository;

public interface IUserRepository
{
    Task<User> AddFilmStudioAsync(FilmStudio filmStudio, User user);

    Task<User> AddUserAsync(User user);
}