using Microsoft.EntityFrameworkCore;
using Project.Business.Exceptions;

namespace Project.Middlewares
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;

        public ExceptionLoggingMiddleware(RequestDelegate next,
                                           ILogger<ExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException dbEx && dbEx.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    ex = new ConflictException("Unique constraint violation (e.g., personal number or phone already exists)"); //if somehow duplicate value goes through our validations, this one catches it on Db-level :)
                }
                _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    ValidationException => StatusCodes.Status400BadRequest,
                    NotFoundException => StatusCodes.Status404NotFound,
                    ConflictException => StatusCodes.Status409Conflict,
                    ForbiddenException => StatusCodes.Status403Forbidden,
                    UnauthorizedException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                var code = ex is ApiException apiEx ? apiEx.Code : "INTERNAL_SERVER_ERROR";

                var errorResponse = new
                {
                    ex.Message,
                    Code = code,
                    TraceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
