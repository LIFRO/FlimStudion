using System.ComponentModel.DataAnnotations;

namespace BackEnd.Dto;

public class FilmStudioLoginDto
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}