namespace BackEnd.Services;

public interface IUserService
{
    public Task<User> RegisterAsync(string name, string Password);

    public Task<User> LoginAsync(string name, string Password);
}