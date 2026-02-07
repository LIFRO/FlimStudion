using System.ComponentModel.DataAnnotations;
using BackEnd.Models;

namespace BackEnd.Dto;

public class UserLoginDto : IuserAuthenticate
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}