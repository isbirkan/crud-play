using System.Reflection;

using CrudPlay.Application.Interfaces;
using CrudPlay.Infrastructure.Interfaces;
using CrudPlay.Infrastructure.Options;
using CrudPlay.Infrastructure.Persistance;
using CrudPlay.Infrastructure.Services;

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

        //services.AddDbContext<TodoDbContext>((serviceProvider, options) =>
        //{
        //    var persistenceOptions = serviceProvider.GetRequiredService<IOptions<PersistenceOptions>>().Value;
        //    options.UseSqlServer(persistenceOptions.ConnectionString);
        //});

        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer("Data Source=BRKN-PC;Initial Catalog=CrudPlay;Integrated Security=True;Encrypt=False"));


        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IRepository<>), typeof(TodoRepository<>));
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }

    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        DatabaseInitializer.InitializeDatabase(serviceProvider);
    }
}
