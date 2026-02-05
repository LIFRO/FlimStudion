using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.API;

[Route("/api/filmstudio")]
[ApiController]
public class UserController : ControllerBase
{
private readonly IUserService _ users
public UserController(IUserService user)
    {
        _users = users;
    }

    public Task<IEnumerable<FilmStudio>>
}