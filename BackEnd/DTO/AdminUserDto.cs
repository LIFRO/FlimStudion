using BackEnd.Models;

namespace BackEnd.Dto;

public class AdminUserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Role { get; set; } = "";
}