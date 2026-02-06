using BackEnd.Dto;
using BackEnd.Models;



namespace BackEnd.Services;

public interface IUserService
{
    public Task<User> RegisterAsync(UserRegisterDto request, string role);

    //public Task<User> LoginAsync(string name, string Password);
}