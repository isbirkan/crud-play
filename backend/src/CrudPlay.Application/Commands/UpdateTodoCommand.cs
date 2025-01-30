using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.DTO;

using MediatR;

using Microsoft.Extensions.Logging;

namespace CrudPlay.Application.Commands;

public record UpdateTodoCommand(string Id, UpdateTodoRequest request) : IRequest;

public class UpdateTodoCommandHandler(
    ILogger<UpdateTodoCommandHandler> logger,
    IUpdateTodoCommandValidator validator,
    ITodoService service)
    : IRequestHandler<UpdateTodoCommand>
{
    private readonly ILogger<UpdateTodoCommandHandler> _logger = logger;
    private readonly IUpdateTodoCommandValidator _validator = validator;
    private readonly ITodoService _service = service;

    public async Task Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start query: {nameof(UpdateTodoCommand)}");
        _validator.ValidateOrThrowException(command);

        await _service.UpdateAsync(command.Id, command.request, cancellationToken);
    }
}
