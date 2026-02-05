namespace BackEnd.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = ""; 
    public int? FilmStudioId { get; set; }
    public IFilmStudio? FilmStudio { get; set; }
}