1. Add migration 
    dotnet ef migrations add initial -c UserDbContext --startup-project ../Jkulds.Micro.Auth.Api/Jkulds.Micro.Auth.Api.csproj

2. Database update 
    dotnet ef database update -c UserDbContext --startup-project ../Jkulds.Micro.Auth.Api/Jkulds.Micro.Auth.Api.csproj

