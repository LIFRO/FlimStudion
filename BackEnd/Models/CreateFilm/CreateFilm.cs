using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Models;

public class CreateFilm
{
    [Required]
    public string Title { get; set; } = "";
    
    [Required]
    public string Genre { get; set; } = "";

    [Required]
    public int Copies { get; set; }
}