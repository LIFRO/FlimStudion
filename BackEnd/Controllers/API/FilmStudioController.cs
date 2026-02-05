using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers.API;

[Route("/api/filmstudio")]
[ApiController]
public class FilmStudioController  : ControllerBase
{
private readonly IUserService _users;

public FilmStudioController(IUserService Users)
    {
        _users = Users;
    }

    public Task<IEnumerable<FilmStudio>> Register()
    {
        
    }
}