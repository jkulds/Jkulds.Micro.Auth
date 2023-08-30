using Jkulds.Micro.Auth.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jkulds.Micro.Auth.Data;

public static class DataLayerBootstrap
{
    public static async Task BootstrapDb(this IServiceProvider provider)
    {
        await using var scope = provider.CreateAsyncScope();
        
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }
}