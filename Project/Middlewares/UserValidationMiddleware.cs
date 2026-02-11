using DAL.Repositories;
using System.Security.Claims;

public class UserValidationMiddleware
{
    private readonly RequestDelegate _next;

    public UserValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserRepository users)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleInToken = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (id != null && int.TryParse(id, out var userId))
            {
                var user = await users.GetByIdAsync(userId);

                if (user == null ||
                    !string.Equals(user.Role, roleInToken, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
        }

        await _next(context);
    }
}
