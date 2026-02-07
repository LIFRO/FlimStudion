using BackEnd.Models;
using BackEnd.Dto;

namespace BackEnd.Repository;

public interface IUserRepository
{

    Task<User?> FindUser(UserLoginDto request);

    Task<User> AddFilmStudioAsync(FilmStudio filmStudio, User user);

    Task<User> AddUserAsync(User user);

    Task<bool> UserExists(string user);
}