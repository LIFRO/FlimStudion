using BackEnd.Repository;
using BackEnd.Models;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;

namespace BackEnd.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;

    public UserService(IUserRepository users)
    {
        _users = users;
    }

    public static string HashPasswordArgon2id(string password, byte[] salt)
    {
        //Hashes password with Argon2Id with random salt
        var argon = new Argon2id(Encoding.UTF8.GetBytes(password)){
        Salt = salt,
        DegreeOfParallelism = 2,
        MemorySize = 64 * 1024,
        Iterations = 3
        };
        return Convert.ToBase64String(argon.GetBytes(32));
    }

    public Task<User> RegisterAsync(string name, string studioName, string password, string city, string role)
    {
        //Generate a random Salt per user.
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        var filmStudio = new FilmStudio
        {
            StudioName = studioName,
            City = city 
        };
        var user = new User
        {
            UserName = name,
            Password = HashPasswordArgon2id(password, salt),
            Role = role,
            Salt = salt,
            FilmStudio = filmStudio
        };


        return _users.AddFilmStudioAsync(filmStudio, user);

    }

}