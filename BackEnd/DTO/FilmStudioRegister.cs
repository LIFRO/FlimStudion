using System.ComponentModel.DataAnnotations;
using BackEnd.Models;

namespace BackEnd.Dto;

public class FilmStudioRegister
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]

    public string Password { get; set; } = "";

    [Required]
    public string City { get; set; } = "";

    [Required]
    public string studioName { get; set; } = "";
}



