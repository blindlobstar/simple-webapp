# Simple API

## Debug
```
docker compose -f docker-compose.debug.yml up simpleapi --build
```
## Migrations
To add new migration run following script:
```
dotnet ef migrations add <MIGRATION NAME> -p ./src/SimpleWebApp.Data.Migrations/SimpleWebApp.Data.Migrations.csproj
```
## Test
### Integration Tests
```
docker compose -f docker-compose.integration.yml up -V --build --exit-code-from test test
docker compose -f docker-compose.integration.yml down --volumes
```
### Unit Tests
```
dotnet test test/SimpleWebApp.Api.UnitTests
```
### Technologies
`.NET`
`EF Core`
`Postgres`
`FluentValidation`
`Docker`