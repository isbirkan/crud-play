using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Queries;
using CrudPlay.Application.Validators.Interfaces;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace CrudPlay.Application.UnitTests.Queries;

public class GetTodosByUserIdQueryHandlerTests
{
    private readonly ILogger<GetTodosByUserIdQueryHandler> _logger;
    private readonly IGetTodosByUserIdQueryValidator _validator;
    private readonly ITodoService _service;
    private readonly GetTodosByUserIdQueryHandler _handler;

    public GetTodosByUserIdQueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<GetTodosByUserIdQueryHandler>>();
        _validator = Substitute.For<IGetTodosByUserIdQueryValidator>();
        _service = Substitute.For<ITodoService>();
        _handler = new(_logger, _validator, _service);
    }

    [Fact]
    public async Task Handle_ValidationSuccess_UserIdMatches_ShouldReturnTodoList()
    {
        // Arrange
        var userId = "use-my-id";
        var query = new GetTodosByUserIdQuery(userId);
        var cancellationToken = new CancellationToken();

        _service.GetByUserIdAsync(userId, cancellationToken).Returns(
            [
                new(
                    "number-one-id",
                    "My first ever Todo",
                    "I swear! This is my first time!",
                    false,
                    null,
                    69
                    ),
                new(
                    "number-two-id",
                    "Maybe I will second this",
                    "I did like it though",
                    true,
                    null,
                    0
                    )
            ]);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.Equal(2, result.Count());

        var todoList = result.ToList();
        Assert.Equal("My first ever Todo", todoList[0].Title);
        Assert.Equal("I swear! This is my first time!", todoList[0].Description);
        Assert.False(todoList[0].IsCompleted);
        Assert.Null(todoList[0].DueDate);
        Assert.Equal(69, todoList[0].Priority);
        Assert.Equal("Maybe I will second this", todoList[1].Title);
        Assert.Equal("I did like it though", todoList[1].Description);
        Assert.True(todoList[1].IsCompleted);
        Assert.Null(todoList[1].DueDate);
        Assert.Equal(0, todoList[1].Priority);
    }

    [Fact]
    public async Task Handle_ValidationSuccess_UserIdDoesNotMatch_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = "number-one-id";
        var query = new GetTodosByUserIdQuery(userId);
        var cancellationToken = new CancellationToken();

        _service.GetByUserIdAsync(userId, cancellationToken).Returns([]);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.Empty(result);
    }
}
