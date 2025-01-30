using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Domain;

using MediatR;

using Microsoft.Extensions.Logging;

namespace CrudPlay.Application.Queries;

public record GetTodoByIdQuery(string Id) : IRequest<Todo>;

internal class GetTodoByIdHandler(
    ILogger<GetTodoByIdHandler> logger,
    IGetTodoByIdQueryValidator validator,
    ITodoService service)
    : IRequestHandler<GetTodoByIdQuery, Todo>
{
    private readonly ILogger<GetTodoByIdHandler> _logger = logger;
    private readonly IGetTodoByIdQueryValidator _validator = validator;
    private readonly ITodoService _service = service;

    public async Task<Todo> Handle(GetTodoByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start query: {nameof(GetTodoByIdQuery)}");
        _validator.ValidateOrThrowException(query);

        return await _service.GetByIdAsync(query.Id, cancellationToken);
    }
}
