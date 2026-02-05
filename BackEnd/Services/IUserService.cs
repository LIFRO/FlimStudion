using BackEnd.Models;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;


namespace BackEnd.Services;

public interface IUserService
{
    public Task<User> RegisterAsync(string name, string Email, string Password, string role);

    public Task<User> LoginAsync(string name, string Password);
}