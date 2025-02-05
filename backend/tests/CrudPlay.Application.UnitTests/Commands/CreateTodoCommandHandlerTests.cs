using CrudPlay.Application.Commands;
using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.DTO;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace CrudPlay.Application.UnitTests.Commands;

public class CreateTodoCommandHandlerTests
{
    private readonly ILogger<CreateTodoCommandHandler> _logger;
    private readonly ICreateTodoCommandValidator _validator;
    private readonly ITodoService _service;
    private readonly CreateTodoCommandHandler _handler;

    public CreateTodoCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<CreateTodoCommandHandler>>();
        _validator = Substitute.For<ICreateTodoCommandValidator>();
        _service = Substitute.For<ITodoService>();
        _handler = new(_logger, _validator, _service);
    }

    [Fact]
    public async Task Handle_ValidationSuccess_ShouldCreate()
    {
        // Arrange
        var request = new CreateTodoRequest("Awesome Title for a Todo", "Do I really need one?", DateTime.MinValue, 999);
        var command = new CreateTodoCommand(request);
        var cancellationToken = new CancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _validator.Received().ValidateOrThrowException(command);
        await _service.Received().CreateAsync(request, cancellationToken);
    }
}
