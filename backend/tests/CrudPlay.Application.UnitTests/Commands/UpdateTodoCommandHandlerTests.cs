using CrudPlay.Application.Commands;
using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.DTO;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace CrudPlay.Application.UnitTests.Commands;

public class UpdateTodoCommandHandlerTests
{
    private readonly ILogger<UpdateTodoCommandHandler> _logger;
    private readonly IUpdateTodoCommandValidator _validator;
    private readonly ITodoService _service;
    private readonly UpdateTodoCommandHandler _handler;

    public UpdateTodoCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<UpdateTodoCommandHandler>>();
        _validator = Substitute.For<IUpdateTodoCommandValidator>();
        _service = Substitute.For<ITodoService>();
        _handler = new(_logger, _validator, _service);
    }

    [Fact]
    public async Task Handle_ValidationSuccess_ShouldUpdate()
    {
        // Arrange
        var identifier = "Id-not-know-what-I-am-doing-here";
        var request = new UpdateTodoRequest(
            "I would like a new title, please!",
            "Can you describe me better?",
            true,
            new DateTime(3025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            999);
        var command = new UpdateTodoCommand(identifier, request);
        var cancellationToken = new CancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _validator.Received().ValidateOrThrowException(command);
        await _service.Received().UpdateAsync(identifier, request, cancellationToken);
    }
}
