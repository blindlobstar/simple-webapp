FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["test/SimpleWebApp.Api.IntegrationTests/SimpleWebApp.Api.IntegrationTests.csproj", "test/SimpleWebApp.Api.IntegrationTests/"]
RUN dotnet restore "test/SimpleWebApp.Api.IntegrationTests/SimpleWebApp.Api.IntegrationTests.csproj"
COPY . .
WORKDIR "/src/test/SimpleWebApp.Api.IntegrationTests"
RUN dotnet build "SimpleWebApp.Api.IntegrationTests.csproj" -c Release -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SimpleWebApp.Api.IntegrationTests.csproj" -c Release -o /app/publish

FROM publish AS test
ENTRYPOINT ["dotnet", "test"]
