using System.Security.Claims;
using QAPortal.Data.Enums;
using QAPortal.Shared.DTOs.UserDtos;

namespace QAPortal.Presentation.Middlewares;

public class JwtUserMiddleware
{
    private readonly RequestDelegate _next;

    public JwtUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var user = new UserDto
                {
                    UserId = int.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                    UserName = context.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                    Email = context.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                    Role = Enum.TryParse(context.User.FindFirst(ClaimTypes.Role)?.Value, out UserRole role) ? role : UserRole.User,
                };
                context.Items["currentUser"] = user;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error decoding JWT token:");
                System.Console.WriteLine(ex.Message);
            }
        }

        await _next(context);
    }
}