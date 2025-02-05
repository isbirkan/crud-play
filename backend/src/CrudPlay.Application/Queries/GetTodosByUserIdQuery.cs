using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Domain;

using MediatR;

using Microsoft.Extensions.Logging;

namespace CrudPlay.Application.Queries;

public record GetTodosByUserIdQuery(string UserId) : IRequest<IEnumerable<TodoModel>>;

public class GetTodosByUserIdQueryHandler(
    ILogger<GetTodosByUserIdQueryHandler> logger,
    IGetTodosByUserIdQueryValidator validator,
    ITodoService service)
    : IRequestHandler<GetTodosByUserIdQuery, IEnumerable<TodoModel>>
{
    private readonly ILogger<GetTodosByUserIdQueryHandler> _logger = logger;
    private readonly IGetTodosByUserIdQueryValidator _validator = validator;
    private readonly ITodoService _service = service;

    public async Task<IEnumerable<TodoModel>> Handle(GetTodosByUserIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start query: {nameof(GetTodosByUserIdQuery)}");
        _validator.ValidateOrThrowException(query);

        return await _service.GetByUserIdAsync(query.UserId, cancellationToken);
    }
}
