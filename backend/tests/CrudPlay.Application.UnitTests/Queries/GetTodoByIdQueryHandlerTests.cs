using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Queries;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Domain;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace CrudPlay.Application.UnitTests.Queries;

public class GetTodoByIdQueryHandlerTests
{
    private readonly ILogger<GetTodoByIdQueryHandler> _logger;
    private readonly IGetTodoByIdQueryValidator _validator;
    private readonly ITodoService _service;
    private readonly GetTodoByIdQueryHandler _handler;

    public GetTodoByIdQueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<GetTodoByIdQueryHandler>>();
        _validator = Substitute.For<IGetTodoByIdQueryValidator>();
        _service = Substitute.For<ITodoService>();
        _handler = new(_logger, _validator, _service);
    }

    [Fact]
    public async Task Handle_ValidationSuccess_ShouldReturnTodoItem()
    {
        // Arrange
        var identifier = "identify-me";
        var query = new GetTodoByIdQuery(identifier);
        var cancellationToken = new CancellationToken();

        _service.GetByIdAsync(identifier, cancellationToken).Returns(
            new TodoModel(
                "number-one-id",
                "My first ever Todo",
                "I swear! This is my first time!",
                false,
                null, 69)
            );

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("My first ever Todo", result.Title);
        Assert.Equal("I swear! This is my first time!", result.Description);
        Assert.False(result.IsCompleted);
        Assert.Null(result.DueDate);
        Assert.Equal(69, result.Priority);
    }
}
