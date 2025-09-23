
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QAPortal.Shared.DTOs.UserDtos;

namespace QAPortal.Business.Services;


public interface IJWTServices
{
    string GenerateAccessToken(UserDto user);

    string GenerateRefreshToken(int userId);
    Task<UserDto> Authenticate(UserLoginDto userLogin);
}

public class JWTService : IJWTServices
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    public JWTService(IConfiguration config, IUserService userService)
    {
        _config = config;
        _userService = userService;
    }

    // To generate  access token
    public string GenerateAccessToken(UserDto user)
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
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }



    // To generate refresh token

    public string GenerateRefreshToken(int userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("RefreshToken", Guid.NewGuid().ToString())
        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMonths(3),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }



    //To authenticate user
    public async Task<UserDto> Authenticate(UserLoginDto userLogin)
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
}