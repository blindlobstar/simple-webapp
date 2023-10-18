FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/SimpleWebApp.Data.Migrations/SimpleWebApp.Data.Migrations.csproj", "src/SimpleWebApp.Data.Migrations/"]
RUN dotnet restore "src/SimpleWebApp.Data.Migrations/SimpleWebApp.Data.Migrations.csproj"
COPY . .
WORKDIR "/src/src/SimpleWebApp.Data.Migrations"
RUN dotnet build "SimpleWebApp.Data.Migrations.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SimpleWebApp.Data.Migrations.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleWebApp.Data.Migrations.dll"]
