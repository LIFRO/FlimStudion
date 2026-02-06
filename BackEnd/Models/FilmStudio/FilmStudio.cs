namespace BackEnd.Models;

public class FilmStudio : IFilmStudio
{
    public int FilmStudioId { get; set;}
    public string StudioName { get; set;} = "";
    public string City { get; set;} = "";
    public List<FilmCopy> RentedFilms { get; set; } = new List<FilmCopy>();
    }