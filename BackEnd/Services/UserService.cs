using BackEnd.Repository;
using BackEnd.Models;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.ObjectPool;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.AccessControl;

namespace BackEnd.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IConfiguration _config;

    public UserService(IUserRepository users, IConfiguration config)
    {
        _users = users;
        _config = config;
    }

    private string JwtTokenGeneration(User user, IConfiguration config)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
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

    public async Task<User?> RegisterAsync(UserRegisterDto request,  string role)
    {

        if(await _users.UserExists(request.UserName))
            return null;

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
        user.FilmStudio = filmStudio;
        return await _users.AddFilmStudioAsync(filmStudio, user);
        }
        return await _users.AddUserAsync(user);
    }

        public async Task<(User user, String token)?> LoginAsync(UserLoginDto request)
    {
        var user = await _users.FindUser(request);
        if(user == null)
        {
            return null;
        }

        var passwordCheck = HashPasswordArgon2id(request.Password, user.Salt);

        if(user.Password == passwordCheck)
        {
            var token = JwtTokenGeneration(user, _config);
            return (user, token);
        }
        return null;
    }

}