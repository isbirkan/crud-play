using AutoMapper;

using CrudPlay.Core.DTO;
using CrudPlay.Core.Mappers;

using TodoEntity = CrudPlay.Core.Entities.TodoEntity;
using TodoItem = CrudPlay.Core.Domain.TodoModel;

namespace CrudPlay.Core.UnitTests.Mappers;

public class TodoProfileTests
{
    private readonly IMapper _mapper;
    private static readonly Guid _identifier = Guid.NewGuid();
    private readonly TodoEntity _todoEntity = new()
    {
        Id = _identifier,
        Title = "I hope you like my title",
        Description = "This also applies to the fabolous description",
        IsCompleted = false,
        DueDate = DateTime.MinValue,
        Priority = 23,
        CreatedAt = new DateTime(2077, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2077, 1, 2, 0, 0, 0, DateTimeKind.Utc)
    };

    public TodoProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TodoProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_FromTodoEntity_ShouldMapToTodoItem()
    {
        // Act
        var todoItem = _mapper.Map<TodoItem>(_todoEntity);

        // Assert
        Assert.Equal(_todoEntity.Title, todoItem.Title);
        Assert.Equal(_todoEntity.Description, todoItem.Description);
        Assert.Equal(_todoEntity.IsCompleted, todoItem.IsCompleted);
        Assert.Equal(_todoEntity.DueDate, todoItem.DueDate);
        Assert.Equal(_todoEntity.Priority, todoItem.Priority);
    }

    [Fact]
    public void Map_FromCreateTodoRequest_ShouldMapToTodoEntity()
    {
        // Arrange
        var createTodoRequest = new CreateTodoRequest(
            "I hope you like my title",
            "This also applies to the fabolous description",
            DateTime.MaxValue,
            420);

        // Act
        var todoEntity = _mapper.Map<TodoEntity>(createTodoRequest);

        // Assert
        Assert.Equal(createTodoRequest.Title, todoEntity.Title);
        Assert.Equal(createTodoRequest.Description, todoEntity.Description);
        Assert.Equal(createTodoRequest.DueDate, todoEntity.DueDate);
        Assert.Equal(createTodoRequest.Priority, todoEntity.Priority);
        Assert.Null(todoEntity.CreatedAt);
        Assert.Null(todoEntity.UpdatedAt);
    }

    [Fact]
    public void Map_FromUpdateTodoRequest_ShouldMapToTodoEntity()
    {
        // Arrange
        var updateTodoRequest = new UpdateTodoRequest(
            "I hope you like my title",
            "This also applies to the fabolous description",
            IsCompleted: true,
            DateTime.MaxValue,
            420);

        // Act
        _mapper.Map(updateTodoRequest, _todoEntity);

        // Assert
        Assert.Equal(updateTodoRequest.Title, _todoEntity.Title);
        Assert.Equal(updateTodoRequest.Description, _todoEntity.Description);
        Assert.True(_todoEntity.IsCompleted);
        Assert.Equal(updateTodoRequest.DueDate, _todoEntity.DueDate);
        Assert.Equal(updateTodoRequest.Priority, _todoEntity.Priority);
        Assert.Equal(new DateTime(2077, 1, 1, 0, 0, 0, DateTimeKind.Utc), _todoEntity.CreatedAt);
        Assert.Equal(new DateTime(2077, 1, 2, 0, 0, 0, DateTimeKind.Utc), _todoEntity.UpdatedAt);
    }

    [Fact]
    public void Map_FromUpdateTodoRequest_PropertiesNull_ShouldNotMapToTodoEntity()
    {
        // Arrange
        var updateTodoRequest = new UpdateTodoRequest(null, null, null, null, null);

        // Act
        _mapper.Map(updateTodoRequest, _todoEntity);

        // Assert
        Assert.Equal("I hope you like my title", _todoEntity.Title);
        Assert.Equal("This also applies to the fabolous description", _todoEntity.Description);
        Assert.False(_todoEntity.IsCompleted);
        Assert.Equal(DateTime.MinValue, _todoEntity.DueDate);
        Assert.Equal(23, _todoEntity.Priority);
        Assert.Equal(new DateTime(2077, 1, 1, 0, 0, 0, DateTimeKind.Utc), _todoEntity.CreatedAt);
        Assert.Equal(new DateTime(2077, 1, 2, 0, 0, 0, DateTimeKind.Utc), _todoEntity.UpdatedAt);
    }
}
