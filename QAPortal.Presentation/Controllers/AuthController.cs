using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QAPortal.Business.Services;
using QAPortal.Data.Enums;
using QAPortal.Shared.DTOs.UserDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace User.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    public AuthController(IConfiguration config, IUserService userService)
    {
        _config = config;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
    {
        var user = await Authenticate(userLogin);
        SignUpLoginResponseDTO signUpLoginResponseDTO = new SignUpLoginResponseDTO();

        if (user != null)
        {
            var token = GenerateToken(user);
            signUpLoginResponseDTO.User = user;
            signUpLoginResponseDTO.Token = token;
            return Ok(signUpLoginResponseDTO);
        }


        return NotFound("user not found");
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] UserRequestDto userDto)
    {
        System.Console.WriteLine(userDto);
        System.Console.WriteLine(userDto.Role);

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



        var token = GenerateToken(new UserDto
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



    // To generate token
    private string GenerateToken(UserDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())

        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    //To authenticate user
    private async Task<UserDto> Authenticate(UserLoginDto userLogin)
    {

        var isValid = await _userService.AuthenticateUserAsync(userLogin);
        if (!isValid)
        {
            return null;
        }

        var currentUser = await _userService.GetUserByEmailAsync(userLogin.Email);

        if (currentUser != null)
        {
            return currentUser;
        }
        return null;
    }

    /* private UserDto GetCurrentUser()
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
    } */
}
