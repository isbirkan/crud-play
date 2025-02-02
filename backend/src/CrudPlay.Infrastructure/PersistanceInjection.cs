using System.Data;

using CrudPlay.Application.Interfaces;
using CrudPlay.Infrastructure.Interfaces;
using CrudPlay.Infrastructure.Options;
using CrudPlay.Infrastructure.Persistance;
using CrudPlay.Infrastructure.Services;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CrudPlay.Infrastructure;

public static class PersistanceInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<PersistenceOptions>()
                .Bind(configuration.GetSection("PersistenceOptions"))
                .ValidateOnStart();
        services.AddSingleton<IValidateOptions<PersistenceOptions>, PersistenceOptionsValidator>();

        var persistenceOptions = configuration.GetSection("PersistenceOptions").Get<PersistenceOptions>();
        switch (persistenceOptions?.Implementation)
        {
            case ImplementationType.EntityFramework:
                // Register EF Core DbContext
                services.AddDbContext<TodoDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(
                        persistenceOptions.ConnectionString,
                        sqlOptions => sqlOptions.MigrationsAssembly("CrudPlay.Infrastructure")
                                                .MigrationsHistoryTable("__EFMigrationsHistory", "Ef"));
                });

                services.AddScoped(typeof(IRepository<>), typeof(EfTodoRepository<>));
                break;

            case ImplementationType.Dapper:
                // Register Dapper's IDbConnection
                services.AddScoped<IDbConnection>(sp =>
                    new SqlConnection(persistenceOptions.ConnectionString));

                services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));
                break;

            default:
                throw new ArgumentException("Invalid Persistence Implementation.");
        }

        services.AddScoped<ITodoService, TodoService>();

        return services;
    }

    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        var persistenceOptions = serviceProvider.GetRequiredService<IOptions<PersistenceOptions>>().Value;

        switch (persistenceOptions.Implementation)
        {
            case ImplementationType.EntityFramework:
                DatabaseInitializer.InitializeEfDatabase(serviceProvider);
                break;

            case ImplementationType.Dapper:
                DatabaseInitializer.InitializeDapperDatabase(persistenceOptions.ConnectionString);
                break;

            default:
                throw new ArgumentException("Invalid Persistence Implementation.");
        }
    }
}
