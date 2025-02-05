using CrudPlay.Core.Entities;
using CrudPlay.Infrastructure.Persistance.Todo;

using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Infrastructure.UnitTests.Persistence.Todo;

public class EfTodoRepositoryTests
{
    private readonly TodoDbContext _context;
    private readonly EfTodoRepository<TodoEntity> _repository;

    private static readonly Guid _identifier1 = Guid.NewGuid();
    private static readonly Guid _identifier2 = Guid.NewGuid();

    private readonly TodoEntity _todoEntity1 = new()
    {
        Id = _identifier1,
        Title = "Hit me first!",
        Description = "In a sky full of stars, you found me",
        IsCompleted = false,
        DueDate = DateTime.MinValue,
        Priority = 1000000,
        UserId = "user-id-first"
    };
    private readonly TodoEntity _todoEntity2 = new()
    {
        Id = _identifier2,
        Title = "Do not put me on second place!",
        Description = "You already did it, didn't you?",
        IsCompleted = true,
        DueDate = DateTime.MaxValue,
        Priority = 101,
        UserId = "user-id-second"
    };

    public EfTodoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new(options);
        _repository = new(_context);

        _context.Todos.Add(_todoEntity1);
        _context.Todos.Add(_todoEntity2);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnTodoList()
    {
        // Act
        var todos = await _repository.GetListAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Equal(2, todos.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnTodo()
    {
        // Act
        var todo = await _repository.GetByIdAsync(_identifier1, CancellationToken.None);

        // Assert
        Assert.NotNull(todo);
        Assert.Equal(todo.Id, _todoEntity1.Id);
        Assert.Equal(todo.Title, _todoEntity1.Title);
        Assert.Equal(todo.Description, _todoEntity1.Description);
        Assert.Equal(todo.IsCompleted, _todoEntity1.IsCompleted);
        Assert.Equal(todo.DueDate, _todoEntity1.DueDate);
        Assert.Equal(todo.Priority, _todoEntity1.Priority);
        Assert.Equal(todo.UserId, _todoEntity1.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
    {
        // Act
        var todo = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.Null(todo);
    }

    [Fact]
    public async Task GetByPropertyAsync_ExistingUserId_ShouldReturnMatchingTodoList()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.UserId, _todoEntity1.UserId, CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todos.First().Id, _todoEntity1.Id);
        Assert.Equal(todos.First().Title, _todoEntity1.Title);
        Assert.Equal(todos.First().Description, _todoEntity1.Description);
        Assert.Equal(todos.First().IsCompleted, _todoEntity1.IsCompleted);
        Assert.Equal(todos.First().DueDate, _todoEntity1.DueDate);
        Assert.Equal(todos.First().Priority, _todoEntity1.Priority);
        Assert.Equal(todos.First().UserId, _todoEntity1.UserId);
    }

    [Fact]
    public async Task GetByPropertyAsync_NonExistingUserId_ShouldReturnEmptyList()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.UserId, "non-existent-user", CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Empty(todos);
    }

    [Fact]
    public async Task GetByPropertyAsync_IsCompletedFalse_ShouldReturnMatchingTodo()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.IsCompleted, false, CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todos.First().Id, _todoEntity1.Id);
        Assert.False(_todoEntity1.IsCompleted);
    }

    [Fact]
    public async Task GetByPropertyAsync_IsCompletedTrue_ShouldReturnMatchingTodo()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.IsCompleted, true, CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todos.First().Id, _todoEntity2.Id);
        Assert.True(_todoEntity2.IsCompleted);
    }

    [Fact]
    public async Task GetByPropertyAsync_MatchingDueDate_ShouldReturnMatchingTodo()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.DueDate, DateTime.MinValue, CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todos.First().Id, _todoEntity1.Id);
        Assert.Equal(DateTime.MinValue, _todoEntity1.DueDate);
    }

    [Fact]
    public async Task GetByPropertyAsync_NonMatchingDueDate_ShouldReturnEmptyTodoList()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.DueDate, null, CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Empty(todos);
    }

    [Fact]
    public async Task GetByPropertyAsync_MatchingPriority_ShouldReturnMatchingTodo()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.Priority, 1000000, CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todos.First().Id, _todoEntity1.Id);
        Assert.Equal(1000000, _todoEntity1.Priority);
    }

    [Fact]
    public async Task GetByPropertyAsync_NonMatchingPriority_ShouldReturnEmptyTodoList()
    {
        // Act
        var todos = await _repository.GetByPropertyAsync(t => t.Priority, 0, CancellationToken.None);

        // Assert
        Assert.NotNull(todos);
        Assert.Empty(todos);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var todo = new TodoEntity()
        {
            Id = Guid.NewGuid(),
            Title = "Third wheel",
            Description = "Don't even ask!",
            IsCompleted = true,
            DueDate = DateTime.MaxValue,
            Priority = 3,
            UserId = "user-id-third"
        };

        // Act
        await _repository.CreateAsync(todo, CancellationToken.None);

        // Assert
        var todos = await _repository.GetListAsync(CancellationToken.None);
        Assert.NotNull(todos);
        Assert.Equal(3, todos.Count());

        var newTodo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == todo.Id);
        Assert.NotNull(newTodo);
        Assert.Equal(newTodo.Id, todo.Id);
        Assert.Equal(newTodo.Title, todo.Title);
        Assert.Equal(newTodo.Description, todo.Description);
        Assert.Equal(newTodo.IsCompleted, todo.IsCompleted);
        Assert.Equal(newTodo.DueDate, todo.DueDate);
        Assert.Equal(newTodo.Priority, todo.Priority);
        Assert.Equal(newTodo.UserId, todo.UserId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var upcomingTodo = _todoEntity1;
        upcomingTodo.Title = "Hit me first, but changed!";
        upcomingTodo.Description = "In a sky full of stars, you found me, but changed";
        upcomingTodo.IsCompleted = true;
        upcomingTodo.DueDate = DateTime.MinValue;
        upcomingTodo.Priority = 1000001;
        upcomingTodo.UserId = "user-id-first-changed";

        // Act
        await _repository.UpdateAsync(upcomingTodo, CancellationToken.None);

        // Assert
        var changedTodo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == upcomingTodo.Id);
        Assert.NotNull(changedTodo);
        Assert.Equal(upcomingTodo.Id, changedTodo.Id);
        Assert.Equal(upcomingTodo.Title, changedTodo.Title);
        Assert.Equal(upcomingTodo.Description, changedTodo.Description);
        Assert.Equal(upcomingTodo.IsCompleted, changedTodo.IsCompleted);
        Assert.Equal(upcomingTodo.DueDate, changedTodo.DueDate);
        Assert.Equal(upcomingTodo.Priority, changedTodo.Priority);
        Assert.Equal(upcomingTodo.UserId, changedTodo.UserId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Act
        await _repository.DeleteAsync(_identifier1, CancellationToken.None);

        // Assert
        var deletedTodo = await _context.Todos.FindAsync(_identifier1);
        Assert.Null(deletedTodo);
    }
}
