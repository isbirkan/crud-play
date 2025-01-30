using CrudPlay.Application.Commands;
using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace CrudPlay.Application.Tests.Commands;

public class DeleteTodoCommandHandlerTests
{
    private readonly ILogger<DeleteTodoCommandHandler> _logger;
    private readonly IDeleteTodoCommandValidator _validator;
    private readonly ITodoService _service;
    private readonly DeleteTodoCommandHandler _handler;

    public DeleteTodoCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<DeleteTodoCommandHandler>>();
        _validator = Substitute.For<IDeleteTodoCommandValidator>();
        _service = Substitute.For<ITodoService>();
        _handler = new(_logger, _validator, _service);
    }

    [Fact]
    public async Task Handle_ValidationSuccess_ShouldDelete()
    {
        // Arrange
        var identifier = "Id-not-know-what-I-am-doing-here";
        var command = new DeleteTodoCommand(identifier);
        var cancellationToken = new CancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _validator.Received().ValidateOrThrowException(command);
        await _service.Received().DeleteAsync(identifier, cancellationToken);
    }
}
