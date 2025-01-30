using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace CrudPlay.Core;

public static class CoreInjector
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
