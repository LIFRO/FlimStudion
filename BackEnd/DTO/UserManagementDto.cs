using System.ComponentModel.DataAnnotations;

namespace BackEnd.Dto;

public class UserManagementDto
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}

public class FilmStudioLoginDto
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}