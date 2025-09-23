using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QAPortal.Business.Services;
using QAPortal.Shared.DTOs.UserDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace User.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly IJWTServices _jwtService;
    public AuthController(IUserService userService, IJWTServices jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
    {
        var user = await _jwtService.Authenticate(userLogin);
        SignUpLoginResponseDTO signUpLoginResponseDTO = new SignUpLoginResponseDTO();

        if (user != null)
        {
            var token = _jwtService.GenerateAccessToken(user);
            signUpLoginResponseDTO.User = user;
            signUpLoginResponseDTO.Token = token;
            return Ok(signUpLoginResponseDTO);
        }

        return NotFound("user not found");
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] UserRequestDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //Prevent USers with registered Email
        var userWithSameEmail = await _userService.GetUserByEmailAsync(userDto.Email);
        if (userWithSameEmail != null)
        {
            return BadRequest("User with same email already exists");
        }

        var createdUser = await _userService.CreateUserAsync(userDto);

        var token = _jwtService.GenerateAccessToken(new UserDto
        {
            UserId = createdUser.UserId,
            UserName = createdUser.UserName,
            Email = createdUser.Email,
            Role = createdUser.Role
        });

        SignUpLoginResponseDTO signUpLoginResponseDTO = new SignUpLoginResponseDTO
        {
            User = createdUser,
            Token = token
        };

        return Ok(signUpLoginResponseDTO);
    }
}