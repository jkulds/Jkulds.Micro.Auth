using FluentValidation;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;
using Jkulds.Micro.Auth.Api.Validators;

namespace Jkulds.Micro.Auth.Api.Installers;

public static class ValidationInstaller
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserRegistrationRequest>, UserRegistrationRequestValidator>();
        services.AddScoped<IValidator<UserLoginRequest>, UserLoginRequestValidator>();
        services.AddScoped<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
        services.AddScoped<IValidator<UserUpdatePasswordRequest>, UserUpdatePasswordRequestValidator>();
    }
}