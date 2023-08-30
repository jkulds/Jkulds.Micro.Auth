using Jkulds.Micro.Auth.Business.Producers;
using Jkulds.Micro.Auth.Business.Services;
using Jkulds.Micro.Auth.Business.Services.AuthService;
using Microsoft.Extensions.DependencyInjection;

namespace Jkulds.Micro.Auth.Business.Installers;

public static class BusinessLayerInstaller
{
    public static void RegisterBusinessLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<NotificationProducer>();
    }
}