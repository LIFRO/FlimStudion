using BackEnd.Models;

namespace BackEnd.Models;

public interface IFilm
{
    public int FilmId { get; set;}
    public string Title { get; set;}
    public string Genre { get; set;}
    public List<FilmCopy> FilmCopies { get; set;}
};