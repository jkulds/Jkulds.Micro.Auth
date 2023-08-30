using System.Reflection;
using Jkulds.Micro.Auth.Data.Context;
using Jkulds.Micro.Options.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jkulds.Micro.Auth.Postgres.Installers;

public static class NpgsqlInstaller
{
    public static void AddNpgsqlUserDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        var optionName = nameof(PostgresOptions).ToLower().Replace("options", "");
        var options = configuration.GetSection(optionName).Get<PostgresOptions>()!;

        services.AddDbContext<UserDbContext>(
            x =>
            {
                var connectionString = options.GetConnectionString();
                Console.WriteLine(connectionString);
                Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
                Console.WriteLine(Environment.GetEnvironmentVariable("INDOCKER"));
                x.UseNpgsql(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            });
    }
}