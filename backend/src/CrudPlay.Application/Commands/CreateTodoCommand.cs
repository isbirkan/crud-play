using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.DTO;

using MediatR;

using Microsoft.Extensions.Logging;

namespace CrudPlay.Application.Commands;

public record CreateTodoCommand(CreateTodoRequest Request) : IRequest;

internal class CreateTodoCommandHandler(
    ILogger<CreateTodoCommandHandler> logger,
    ICreateTodoCommandValidator validator,
    ITodoService service)
    : IRequestHandler<CreateTodoCommand>
{
    private readonly ILogger<CreateTodoCommandHandler> _logger = logger;
    private readonly ICreateTodoCommandValidator _validator = validator;
    private readonly ITodoService _service = service;

    public async Task Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start query: {nameof(CreateTodoCommand)}");
        _validator.ValidateOrThrowException(command);

        await _service.CreateAsync(command.Request, cancellationToken);
    }
}
