using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Models;

public interface ICreateFilm
{
    string Title { get; set; }
    string Genre { get; set; }
    int Copies { get; set; }
}