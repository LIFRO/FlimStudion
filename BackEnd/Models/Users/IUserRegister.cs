namespace BackEnd.Models;

public interface IUserRegiser
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string City { get; set; }

    public string StudioName { get; set; }

    public bool IsAdmin { get; set; }
}