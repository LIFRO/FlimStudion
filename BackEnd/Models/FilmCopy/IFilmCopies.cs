namespace BackEnd.Models;

public interface IFilmCopy
{
    int FilmCopyId { get; set;}
    int FilmId { get; set;} 
    string LoanedByFilmStudioId { get; set;}
}