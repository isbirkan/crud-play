using CrudPlay.Application.Queries;
using CrudPlay.Application.Validators;
using CrudPlay.Application.Validators.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace CrudPlay.Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(GetTodosQuery).Assembly));

        services.AddSingleton<IGetTodoByIdQueryValidator, GetTodoByIdQueryValidator>();
        services.AddSingleton<IGetTodosByUserIdQueryValidator, GetTodosByUserIdQueryValidator>();
        services.AddSingleton<ICreateTodoCommandValidator, CreateTodoCommandValidator>();
        services.AddSingleton<IUpdateTodoCommandValidator, UpdateTodoCommandValidator>();
        services.AddSingleton<IDeleteTodoCommandValidator, DeleteTodoCommandValidator>();

        return services;
    }
}
