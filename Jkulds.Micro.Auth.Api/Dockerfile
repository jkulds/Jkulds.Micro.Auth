FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Jkulds.Micro.Auth.Api/Jkulds.Micro.Auth.Api.csproj", "Jkulds.Micro.Auth.Api/"]
COPY ["Jkulds.Micro.Base/Jkulds.Micro.Base.csproj", "Jkulds.Micro.Base/"]
COPY ["Jkulds.Micro.MessageContracts/Jkulds.Micro.MessageContracts.csproj", "Jkulds.Micro.MessageContracts/"]
COPY ["Jkulds.Micro.Auth.Data/Jkulds.Micro.Auth.Data.csproj", "Jkulds.Micro.Auth.Data/"]
COPY ["Jkulds.Micro.Auth.Business/Jkulds.Micro.Auth.Business.csproj", "Jkulds.Micro.Auth.Business/"]
COPY ["Jkulds.Micro.Options/Jkulds.Micro.Options.csproj", "Jkulds.Micro.Options/"]
COPY ["Jkulds.Micro.Auth.Postgres/Jkulds.Micro.Auth.Postgres.csproj", "Jkulds.Micro.Auth.Postgres/"]
RUN dotnet restore "Jkulds.Micro.Auth.Api/Jkulds.Micro.Auth.Api.csproj"
COPY . .
WORKDIR "/src/Jkulds.Micro.Auth.Api"
RUN dotnet build "Jkulds.Micro.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jkulds.Micro.Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jkulds.Micro.Auth.Api.dll"]
