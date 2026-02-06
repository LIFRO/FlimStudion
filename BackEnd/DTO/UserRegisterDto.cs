using System.ComponentModel.DataAnnotations;
using BackEnd.Models;

namespace BackEnd.Dto;

public class UserRegisterDto
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    public string City { get; set; } = "";

    public string StudioName { get; set; } = "";
}



