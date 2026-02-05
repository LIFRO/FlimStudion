using BackEnd.Models;

namespace BackEnd.Models;

public class Film : IFilm
{
    public int MovieId { get; set;}
    public string Title { get; set;} = "";
    public string Genre { get; set;} = "";
    public IList<IFilmCopy> FilmCopies { get; set;} = new List<IFilmCopy>();
};