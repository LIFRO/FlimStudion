using BackEnd.Models;

namespace BackEnd.Dto;

public class FilmStudioDto
{
    public int UserId { get; set; }
    public string UserName{ get; set;} = "";
    public string Role { get; set; } = "";
    public FilmStudio? filmStudio { get; set; }
}