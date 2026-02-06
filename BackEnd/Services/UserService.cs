using BackEnd.Repository;
using BackEnd.Models;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Dto;

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

    public Task<User> RegisterAsync(UserRegisterDto request,  string role)
    {

        //Generate a random Salt per user.
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        var user = new User
        {
            UserName = request.UserName,
            Password = HashPasswordArgon2id(request.Password, salt),
            Role = role,
            Salt = salt,
        };


        if(role == "filmstudio"){
        var filmStudio = new FilmStudio{};
        filmStudio.StudioName = request.StudioName;
        filmStudio.City = request.City;
        return _users.AddFilmStudioAsync(filmStudio, user);
        }

        return _users.AddUserAsync(user);
    }
}