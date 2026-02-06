using BackEnd.Models;



namespace BackEnd.Services;

public interface IUserService
{
    public Task<User> RegisterAsync(string name, string Email, string Password, string role, string city);

    //public Task<User> LoginAsync(string name, string Password);
}