namespace BackEnd.Models;

public interface IFilmCopy
{
    int FilmCopyId { get; set;}
    int FilmId { get; set;} 
    int? LoanedByFilmStudioId { get; set;}
}