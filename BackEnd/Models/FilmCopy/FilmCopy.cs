namespace BackEnd.Models;

public class FilmCopy : IFilmCopy
{
    public int FilmCopyId { get; set;}
    public int FilmId { get; set;}
    public string LoanedByFilmStudioId { get; set;} = "";
}