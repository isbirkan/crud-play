using CrudPlay.Infrastructure.Persistance.Identity;
using CrudPlay.Infrastructure.Persistance.Todo;

using DbUp;
using DbUp.Engine;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrudPlay.Infrastructure.Persistance;

public static class DatabaseInitializer
{
    public static void InitializeIdentityDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var identityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        identityDbContext.Database.Migrate();
    }

    public static void InitializeEfDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

        dbContext.Database.Migrate();
    }

    public static void InitializeDapperDatabase(string connectionString)
    {
        var srcDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
        if (srcDirectory == null || !Directory.Exists(srcDirectory))
        {
            throw new DirectoryNotFoundException("Could not locate the `src` directory.");
        }

        var migrationsPath = Path.Combine(srcDirectory, "CrudPlay.Infrastructure", "Migrations", "Dapper");
        if (!Directory.Exists(migrationsPath))
        {
            throw new DirectoryNotFoundException($"Migrations directory not found: {migrationsPath}");
        }

        var sqlScripts = Directory.GetFiles(migrationsPath, "*.sql")
                                  .OrderBy(f => f)
                                  .Select(file => new SqlScript(Path.GetFileName(file), File.ReadAllText(file)));
        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScripts(sqlScripts)
            .LogToConsole()
            .JournalToSqlTable("dbo", "SchemaVersions")
            .WithTransactionPerScript()
            .WithVariablesDisabled()
            .WithExecutionTimeout(TimeSpan.FromMinutes(5))
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new Exception($"Database migration failed: {result.Error}");
        }
    }
}
