using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Queries;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace CrudPlay.Application.Tests.Queries;

public class GetTodosQueryHandlerTests
{
    private readonly ILogger<GetTodosQueryHandler> _logger;
    private readonly ITodoService _service;
    private readonly GetTodosQueryHandler _handler;

    public GetTodosQueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<GetTodosQueryHandler>>();
        _service = Substitute.For<ITodoService>();
        _handler = new(_logger, _service);
    }

    [Fact]
    public async Task Handle_ValidationSuccess_ShouldReturnTodoList()
    {
        // Arrange
        var query = new GetTodosQuery();
        var cancellationToken = new CancellationToken();

        _service.GetAllAsync(cancellationToken).Returns(
            [
                new("My first ever Todo", "I swear! This is my first time!", false, null, 69),
                new("Maybe I will second this", "I did like it though", true, null, 0)
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
}
