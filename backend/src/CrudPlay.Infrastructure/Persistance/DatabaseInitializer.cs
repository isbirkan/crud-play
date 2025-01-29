using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrudPlay.Infrastructure.Persistance;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

        // Creates the database if not exists, but keeps schema as it is
        //dbContext.Database.EnsureCreated();

        // Migrates the database to the latest version
        dbContext.Database.Migrate();
    }
}
