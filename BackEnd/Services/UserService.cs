using BackEnd.Models;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;

namespace BackEnd.Services;

public class UserService
{
    public static string HashPasswordArgon2id(string password, byte[] salt)
    {
        var argon = new Argon2id(Encoding.UTF8.GetBytes(password)){
        Salt = salt,
        DegreeOfParallelism = 2,
        MemorySize = 64 * 1024,
        Iterations = 3
        };
        return Convert.ToBase64String(argon.GetBytes(32));
    }

    public Task<User> RegisterAsync(string name, string email, string password, string role)
    {
        
    }

    public Task<User> LoginAsync(string name, string Password)
    {
        
    }
}