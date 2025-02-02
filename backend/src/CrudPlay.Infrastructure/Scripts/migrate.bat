@echo off

echo Applying EF Migrations...
dotnet ef database update -p src/CrudPlay.Infrastructure -s src/CrudPlay.Api

echo Applying Dapper Migrations...
dotnet run --project src/CrudPlay.Api