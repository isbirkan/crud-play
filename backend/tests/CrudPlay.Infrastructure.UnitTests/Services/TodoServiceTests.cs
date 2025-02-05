using System.Linq.Expressions;

using AutoMapper;

using CrudPlay.Core.DTO;
using CrudPlay.Core.Exceptions;
using CrudPlay.Infrastructure.Interfaces;
using CrudPlay.Infrastructure.Services;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using TodoEntity = CrudPlay.Core.Entities.TodoEntity;
using TodoItem = CrudPlay.Core.Domain.TodoModel;

namespace CrudPlay.Infrastructure.UnitTests.Services;

public class TodoServiceTests
{
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IRepository<TodoEntity> _repository = Substitute.For<IRepository<TodoEntity>>();
    private readonly TodoService _service;

    private static readonly Guid _identifier1 = Guid.NewGuid();
    private static readonly Guid _identifier2 = Guid.NewGuid();

    private readonly TodoEntity _todoEntity1 = new()
    {
        Id = _identifier1,
        Title = "First in line",
        Description = "Do I really need one?",
        IsCompleted = false,
        DueDate = DateTime.MinValue,
        Priority = 1,
        UserId = "123-come-with-me"
    };

    private readonly TodoEntity _todoEntity2 = new()
    {
        Id = _identifier2,
        Title = "Second in line",
        Description = "Still the same Dre",
        IsCompleted = true,
        DueDate = DateTime.MaxValue,
        Priority = 9000,
        UserId = "456-theres-no-fix"
    };

    public TodoServiceTests()
    {
        _service = new(_mapper, _repository);
    }

    [Fact]
    public async Task GetAllAsync_RepositoryFailure_ShouldThrowException()
    {
        // Arrange
        _repository.GetListAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("An error occurred while retrieving the Todos list"));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await _service.GetAllAsync(CancellationToken.None));

        // Assert
        Assert.Equal("An error occurred while retrieving the Todos list", exception.Message);
    }

    [Fact]
    public async Task GetAllAsync_TodoListEmpty_ShouldThrowNotFoundException()
    {
        // Arrange
        _repository.GetListAsync(Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _service.GetAllAsync(CancellationToken.None));

        // Assert
        Assert.Equal("No Todo items found", exception.Message);
    }

    [Fact]
    public async Task GetAllAsync_ResponseSuccess_ShouldReturnMappedTodoList()
    {
        // Arrange
        var todoListEntity = new List<TodoEntity> { _todoEntity1, _todoEntity2 };

        _mapper.Map<IEnumerable<TodoItem>>(Arg.Any<IEnumerable<TodoEntity>>())
            .Returns(todoListEntity.Select(entity => new TodoItem
            (
                entity.Id.ToString(),
                entity.Title,
                entity.Description,
                entity.IsCompleted,
                entity.DueDate,
                entity.Priority
            )));
        _repository.GetListAsync(Arg.Any<CancellationToken>()).Returns(todoListEntity);

        // Act
        var result = await _service.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        var todoListModel = result.ToList();
        Assert.Equal("First in line", todoListModel[0].Title);
        Assert.Equal("Do I really need one?", todoListModel[0].Description);
        Assert.False(todoListModel[0].IsCompleted);
        Assert.Equal(DateTime.MinValue, todoListModel[0].DueDate);
        Assert.Equal(1, todoListModel[0].Priority);
    }

    [Fact]
    public async Task GetByIdAsync_RepositoryFailure_ShouldThrowException()
    {
        // Arrange
        _repository.GetByIdAsync(_identifier1, Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("An error occurred while retrieving the Todos item"));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await _service.GetByIdAsync(_identifier1.ToString(), CancellationToken.None));

        // Assert
        Assert.Equal("An error occurred while retrieving the Todos item", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_TodoItemNull_ShouldThrowNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(_identifier1, Arg.Any<CancellationToken>()).Returns((TodoEntity?)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _service.GetByIdAsync(_identifier1.ToString(), CancellationToken.None));

        // Assert
        Assert.Equal("No corresponding Todo item found", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ResponseSuccess_ShouldReturnMappedTodoItem()
    {
        // Arrange
        _mapper.Map<TodoItem>(_todoEntity1)
            .Returns(
            new TodoItem
            (
                _todoEntity1.Id.ToString(),
                _todoEntity1.Title,
                _todoEntity1.Description,
                _todoEntity1.IsCompleted,
                _todoEntity1.DueDate,
                _todoEntity1.Priority
            ));
        _repository.GetByIdAsync(_identifier1, Arg.Any<CancellationToken>()).Returns(_todoEntity1);

        // Act
        var result = await _service.GetByIdAsync(_identifier1.ToString(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("First in line", result.Title);
        Assert.Equal("Do I really need one?", result.Description);
        Assert.False(result.IsCompleted);
        Assert.Equal(DateTime.MinValue, result.DueDate);
        Assert.Equal(1, result.Priority);
    }

    [Fact]
    public async Task GetByUserIdAsync_RepositoryFailure_ShouldThrowException()
    {
        // Arrange
        _repository.GetByPropertyAsync(Arg.Any<Expression<Func<TodoEntity, string>>>(), _todoEntity1.UserId, Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("An error occurred while retrieving the Todos list for user"));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await _service.GetByUserIdAsync(_todoEntity1.UserId, CancellationToken.None));

        // Assert
        Assert.Equal("An error occurred while retrieving the Todos list for user", exception.Message);
    }

    [Fact]
    public async Task GetByUserIdAsync_TodoListEmpty_ShouldThrowNotFoundException()
    {
        // Arrange
        _repository.GetByPropertyAsync(Arg.Any<Expression<Func<TodoEntity, string>>>(), _todoEntity1.UserId, Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _service.GetByUserIdAsync(_todoEntity1.UserId, CancellationToken.None));

        // Assert
        Assert.Equal("No Todo items found", exception.Message);
    }

    [Fact]
    public async Task GetByUserIdAsync_ResponseSuccess_ShouldReturnMappedTodoList()
    {
        // Arrange
        var todoListEntity = new List<TodoEntity> { _todoEntity1, _todoEntity2 };

        _mapper.Map<IEnumerable<TodoItem>>(Arg.Any<IEnumerable<TodoEntity>>())
            .Returns(todoListEntity.Select(entity => new TodoItem
            (
                entity.Id.ToString(),
                entity.Title,
                entity.Description,
                entity.IsCompleted,
                entity.DueDate,
                entity.Priority
            )));
        _repository.GetByPropertyAsync(Arg.Any<Expression<Func<TodoEntity, string>>>(), _todoEntity1.UserId, Arg.Any<CancellationToken>()).Returns(todoListEntity);

        // Act
        var result = await _service.GetByUserIdAsync(_todoEntity1.UserId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        var todoListModel = result.ToList();
        Assert.Equal("First in line", todoListModel[0].Title);
        Assert.Equal("Do I really need one?", todoListModel[0].Description);
        Assert.False(todoListModel[0].IsCompleted);
        Assert.Equal(DateTime.MinValue, todoListModel[0].DueDate);
        Assert.Equal(1, todoListModel[0].Priority);
    }

    [Fact]
    public async Task CreateAsync_ResponseSuccess_ShouldCreateTodoItem()
    {
        // Arrange
        var createTodoRequest = new CreateTodoRequest("I eager creation", "Please do it already!", null, 11);

        _mapper.Map<TodoEntity>(createTodoRequest).Returns(_todoEntity1);

        // Act
        await _service.CreateAsync(createTodoRequest, CancellationToken.None);

        // Assert
        await _repository.Received().CreateAsync(_todoEntity1, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_TodoItemNull_ShouldThrowNotFoundException()
    {
        // Arrange
        var updateTodoRequest = new UpdateTodoRequest(null, null, null, null, null);

        _mapper.Map(updateTodoRequest, _todoEntity1).Returns(_todoEntity1);
        _repository.GetByIdAsync(_identifier1, Arg.Any<CancellationToken>()).Returns((TodoEntity?)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _service.UpdateAsync(_identifier1.ToString(), updateTodoRequest, CancellationToken.None));

        // Assert
        Assert.Equal("No corresponding Todo item found", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_ResponseSuccess_ShouldUpdateTodoItem()
    {
        // Arrange
        var updateTodoRequest = new UpdateTodoRequest("Nobody will know", "I am surely a broken test that took a long time to fix", false, DateTime.MinValue, 12321);
        _mapper.Map<TodoEntity>(updateTodoRequest).Returns(_todoEntity1);
        _repository.GetByIdAsync(_identifier1, Arg.Any<CancellationToken>()).Returns(_todoEntity1);

        // Act
        await _service.UpdateAsync(_identifier1.ToString(), updateTodoRequest, CancellationToken.None);

        // Assert
        await _repository.Received().UpdateAsync(_todoEntity1, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_TodoItemNull_ShouldThrowNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(_identifier1, Arg.Any<CancellationToken>()).Returns((TodoEntity?)null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _service.DeleteAsync(_identifier1.ToString(), CancellationToken.None));

        // Assert
        Assert.Equal("No corresponding Todo item found", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_ResponseSuccess_ShouldDeleteTodoItem()
    {
        // Arrange
        _repository.GetByIdAsync(_identifier1, Arg.Any<CancellationToken>()).Returns(_todoEntity1);

        // Act
        await _service.DeleteAsync(_identifier1.ToString(), CancellationToken.None);

        // Assert
        await _repository.Received().DeleteAsync(_identifier1, Arg.Any<CancellationToken>());
    }
}
