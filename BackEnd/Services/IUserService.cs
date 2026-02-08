using BackEnd.Dto;
using BackEnd.Models;



namespace BackEnd.Services;

public interface IUserService
{
    Task<User?> RegisterAsync(UserRegisterDto request, string role);

    Task<(User user, String token)?> LoginAsync(UserLoginDto request);
}