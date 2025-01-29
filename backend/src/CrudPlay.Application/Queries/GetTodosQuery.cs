using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Models;

using MediatR;

using Microsoft.Extensions.Logging;

namespace CrudPlay.Application.Queries;

public record GetTodosQuery : IRequest<IEnumerable<TodoItem>>;

internal class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IEnumerable<TodoItem>>
{
    private readonly ILogger<IRequest> _logger;
    private readonly ITodoService _service;

    public GetTodosQueryHandler(ILogger<IRequest> logger, ITodoService service)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoItem>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start query: {nameof(GetTodosQuery)}");

        return await _service.GetAllAsync(cancellationToken);
    }
}
