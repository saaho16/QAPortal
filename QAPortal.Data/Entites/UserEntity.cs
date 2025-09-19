using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Enums;

namespace QAPortal.Data.Entities;

public class UserEntity
{

    [Key]
    public int UserId { get; set; }

    public string UserName { get; set; }

    [EmailAddress]

    public string Email { get; set; }

    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }


}



