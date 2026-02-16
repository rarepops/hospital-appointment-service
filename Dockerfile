FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Hospital.sln .
COPY global.json .
COPY Hospital.Domain/Hospital.Domain.csproj Hospital.Domain/
COPY Hospital.Application/Hospital.Application.csproj Hospital.Application/
COPY Hospital.Infrastructure/Hospital.Infrastructure.csproj Hospital.Infrastructure/
COPY Hospital.ServiceDefaults/Hospital.ServiceDefaults.csproj Hospital.ServiceDefaults/
COPY Hospital.WebApi/Hospital.WebApi.csproj Hospital.WebApi/
COPY Hospital.AppHost/Hospital.AppHost.csproj Hospital.AppHost/

# Restore dependencies
RUN dotnet restore Hospital.WebApi/Hospital.WebApi.csproj

# Copy everything and build
COPY . .
RUN dotnet publish Hospital.WebApi/Hospital.WebApi.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Hospital.WebApi.dll"]
