using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;

using MediatR;

using Microsoft.Extensions.Logging;

namespace CrudPlay.Application.Commands;

public record DeleteTodoCommand(string Id) : IRequest;

internal class DeleteTodoCommandHandler(
    ILogger<DeleteTodoCommandHandler> logger,
    IDeleteTodoCommandValidator validator,
    ITodoService service)
    : IRequestHandler<DeleteTodoCommand>
{
    private readonly ILogger<DeleteTodoCommandHandler> _logger = logger;
    private readonly IDeleteTodoCommandValidator _validator = validator;
    private readonly ITodoService _service = service;

    public async Task Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start query: {nameof(DeleteTodoCommand)}");
        _validator.ValidateOrThrowException(command);

        await _service.DeleteAsync(command.Id, cancellationToken);
    }
}

