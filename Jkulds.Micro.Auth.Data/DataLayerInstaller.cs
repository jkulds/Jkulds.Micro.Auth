using Jkulds.Micro.Auth.Data.Context;
using Jkulds.Micro.Auth.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Jkulds.Micro.Auth.Data;

public static class DataLayerInstaller
{
    public static void AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApiUser, ApiRole>
            (
                x =>
                {
                    x.Password.RequireDigit = true;
                    x.Password.RequiredLength = 6;
                    x.Password.RequireLowercase = false;
                    x.Password.RequireNonAlphanumeric = false;
                    x.Password.RequireUppercase = false;
                    x.User.RequireUniqueEmail = true;
                    x.SignIn.RequireConfirmedEmail = false;
                    x.SignIn.RequireConfirmedAccount = false;
                }
            )
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();
    }
}