using System.Data;

using CrudPlay.Application.Interfaces;
using CrudPlay.Core.Options;
using CrudPlay.Infrastructure.Interfaces;
using CrudPlay.Infrastructure.Persistance;
using CrudPlay.Infrastructure.Persistance.Identity;
using CrudPlay.Infrastructure.Persistance.Todo;
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
        services.AddOptions(configuration);

        var persistenceOptions = configuration.GetSection("PersistenceOptions").Get<PersistenceOptions>();
        services.AddIdentity(persistenceOptions);
        services.AddPersistance(persistenceOptions);

        services.AddScoped<ITodoService, TodoService>();

        return services;
    }

    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        DatabaseInitializer.InitializeIdentityDatabase(serviceProvider);

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

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<PersistenceOptions>()
            .Bind(configuration.GetSection("PersistenceOptions"))
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<PersistenceOptions>, PersistenceOptionsValidator>();
        services.AddOptions<JwtOptions>()
                .Bind(configuration.GetSection("JwtConfiguration"))
                .ValidateOnStart();
        services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services, PersistenceOptions? persistenceOptions)
    {
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(
                persistenceOptions?.ConnectionString,
                sqlOptions => sqlOptions.MigrationsAssembly("CrudPlay.Infrastructure")
                                        .MigrationsHistoryTable("__IdentityMigrationsHistory", "Ef"));
        });

        return services;
    }

    private static IServiceCollection AddPersistance(this IServiceCollection services, PersistenceOptions? persistenceOptions)
    {
        switch (persistenceOptions?.Implementation)
        {
            case ImplementationType.EntityFramework:
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
                services.AddScoped<IDbConnection>(sp => new SqlConnection(persistenceOptions.ConnectionString));
                services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));
                break;

            default:
                throw new ArgumentException("Invalid Persistence Implementation.");
        }

        return services;
    }
}
