namespace BackEnd.Models;

public interface IFilmStudio
{
    public int FilmStudioId { get; set;}
    public string StudioName { get; set;}
    public string City { get; set;}
    public List<FilmCopy> RentedFilms { get; set;}
}