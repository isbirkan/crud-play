@echo off

::dotnet ef migrations add InitialCreate -p src/CrudPlay.Infrastructure -s src/CrudPlay.Api --context ApplicationIdentityDbContext --output-dir Migrations/Identity
::dotnet ef migrations add InitialCreate -p src/CrudPlay.Infrastructure -s src/CrudPlay.Api --context TodoDbContext --output-dir Migrations/Ef

echo Applying EF Migrations...
dotnet ef database update -p src/CrudPlay.Infrastructure -s src/CrudPlay.Api --context ApplicationIdentityDbContext
dotnet ef database update -p src/CrudPlay.Infrastructure -s src/CrudPlay.Api --context TodoDbContext

echo Applying Dapper Migrations...
dotnet run --project src/CrudPlay.Api