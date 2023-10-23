using Jkulds.Micro.Auth.Api.Installers;
using Jkulds.Micro.Auth.Api.Middleware;
using Jkulds.Micro.Auth.Business.Installers;
using Jkulds.Micro.Auth.Data;
using Jkulds.Micro.Auth.Postgres.Installers;
using Jkulds.Micro.Options.Installers;
using Jkulds.Micro.Options.Jwt;
using Jkulds.Micro.Options.MassTransit;
using Jkulds.Micro.Options.Postgres;
using Jkulds.Micro.Options.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExternalServiceOptions(builder.Configuration, new List<Type>
{
    typeof(JwtOptions),
    typeof(RedisOptions),
    typeof(PostgresOptions)
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddNpgsqlUserDbContext(builder.Configuration);

builder.Services.AddCustomIdentity();

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.RegisterValidators();

builder.Services.RegisterBusinessLayerServices();

builder.Services.AddMassTransitWithRabbitMq();

builder.Services.AddLogging();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenWithJwtAuth();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<UnauthorizedMessageMiddleware>();

await app.Services.BootstrapDb();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapControllers();

app.Run();