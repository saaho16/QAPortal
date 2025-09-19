using System.ComponentModel.DataAnnotations;
using QAPortal.Data.Enums;

namespace QAPortal.Shared.DTOs.UserDtos;

public class UserDto
{
    
    public int UserId { get; set; }
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public UserRole Role { get; set; }
}


public class UserRequestDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public UserRole Role { get; set; }
}

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}




public class SignUpLoginResponseDTO
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}