using Jkulds.Micro.Auth.Data.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jkulds.Micro.Auth.Data.Context;

//   from terminal of adapter example NPGSQL
//   dotnet ef migrations add initial -c UserDbContext --startup-project ../Jkulds.Micro.Auth.Api/Jkulds.Micro.Auth.Api.csproj
//   dotnet ef database update -c UserDbContext --startup-project ../Jkulds.Micro.Auth.Api/Jkulds.Micro.Auth.Api.csproj
//
public class UserDbContext : IdentityDbContext<ApiUser, ApiRole, Guid>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
}