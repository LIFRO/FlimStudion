using System.ComponentModel.DataAnnotations;

namespace BackEnd.Dto;

public class UserLoginDto
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}