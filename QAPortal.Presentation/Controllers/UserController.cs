
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QAPortal.Business.Services;
using QAPortal.Data.Enums;
using QAPortal.Shared.DTOs.UserDtos;
namespace User.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [Authorize(Roles ="Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [Authorize(Roles ="Admin,User")]
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDto userDto)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        var updatedUser = await _userService.UpdateUserAsync(userDto);
        System.Console.WriteLine(updatedUser);
        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(updatedUser);
    }


    [Authorize(Roles ="Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }


    [HttpGet("get")]
    public async Task<IActionResult> getUserDetails()
    {
        var user = GetCurrentUser();
        if (user == null)
        {
            return NotFound();
            
        }
        return Ok(user);
    }
    private UserDto GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return new UserDto
            {
                UserId = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value!),
                UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value!,
                Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value!,
                Role = Enum.TryParse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value, out UserRole role) ? role : UserRole.User
            };
        }
        return null;
    }

}

