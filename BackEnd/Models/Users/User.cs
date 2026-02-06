namespace BackEnd.Models;

public class User : IUser
{
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = ""; 
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    public FilmStudio? FilmStudio { get; set; }
}