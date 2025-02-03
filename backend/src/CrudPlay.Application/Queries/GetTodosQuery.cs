using CrudPlay.Application.Interfaces;
using CrudPlay.Core.Domain;

using MediatR;

using Microsoft.Extensions.Logging;

namespace CrudPlay.Application.Queries;

public record GetTodosQuery : IRequest<IEnumerable<TodoModel>>;

public class GetTodosQueryHandler(
    ILogger<GetTodosQueryHandler> logger,
    ITodoService service)
    : IRequestHandler<GetTodosQuery, IEnumerable<TodoModel>>
{
    private readonly ILogger<GetTodosQueryHandler> _logger = logger;
    private readonly ITodoService _service = service;

    public async Task<IEnumerable<TodoModel>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start query: {nameof(GetTodosQuery)}");

        return await _service.GetAllAsync(cancellationToken);
    }
}
