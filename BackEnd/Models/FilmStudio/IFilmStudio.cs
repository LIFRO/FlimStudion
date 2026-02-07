namespace BackEnd.Models;

public interface IFilmStudio
{
    int FilmStudioId { get; set;}
    string StudioName { get; set;}
    string City { get; set;}
    List<FilmCopy> RentedFilms { get; set;}
}