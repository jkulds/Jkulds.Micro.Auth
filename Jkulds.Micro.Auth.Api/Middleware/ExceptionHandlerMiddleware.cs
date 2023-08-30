using System.Net;
using System.Text.Json;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Response;
using Jkulds.Micro.Auth.Business.Services.Exceptions;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api;

namespace Jkulds.Micro.Auth.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
        catch (Exception exception)
        {
            _logger.LogError(exception, "error during executing {Context}", context.Request.Path.Value);
            var response = context.Response;
            response.ContentType = "application/json";
            var localizationKey = "unknown_error";
            var status = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case ArgumentException:
                case InvalidOperationException:
                {
                    localizationKey = "invalid_operation";
                    status = HttpStatusCode.InternalServerError;

                    break;
                }
                case UserRegistrationException:
                case UserLoginException:
                {
                    localizationKey = "unauthorized";
                    status = HttpStatusCode.Unauthorized;

                    break;
                }
            }
            
            /*
            string message;
            try
            {
                // Todo localization service
                var language = context.User.Claims.FirstOrDefault(c => c.Type == "language")?.Value;

                /*if (!Enum.TryParse(language, out ELanguage eLanguage))
                {
                    eLanguage = ELanguage.EN;
                }
                
                message = _localizationService.GetLocalizedStringByLanguage(eLanguage, localizationKey);#1#
            }
            catch
            {
                message = "Unknown error";
            }
            */

            response.StatusCode = (int)status;
            
            var error = new ErrorResponse(localizationKey);
            
            var serializedError = error.ToString();

            await response.WriteAsync(serializedError);
        }
    }
}