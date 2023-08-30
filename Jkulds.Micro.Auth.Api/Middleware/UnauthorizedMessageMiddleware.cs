using Jkulds.Micro.Auth.Api.Controllers.Models.Api;

namespace Jkulds.Micro.Auth.Api.Middleware;

public class UnauthorizedMessageMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedMessageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
        
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            context.Response.ContentType = "application/json";

            var error = new ErrorResponse("unauthorized");
            var serializedError = error.ToString();
            
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            
            await context.Response.WriteAsync(serializedError);
        }
    }
}