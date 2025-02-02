using CrudPlay.Core.Entities;
using CrudPlay.Infrastructure.Persistance;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrudPlay.Infrastructure.UnitTests.Persistence;

public class EfTodoRepositoryTests
{
    private readonly TodoDbContext _context;
    private readonly EfTodoRepository<Todo> _repository;

    private static readonly Guid _identifier1 = Guid.NewGuid();
    private static readonly Guid _identifier2 = Guid.NewGuid();

    private readonly Todo _todoEntity1 = new()
    {
        Id = _identifier1,
        Title = "Hit me first!",
        Description = "In a sky full of stars, you found me",
        IsCompleted = false,
        DueDate = DateTime.MinValue,
        Priority = 1000000
    };
    private readonly Todo _todoEntity2 = new()
    {
        Id = _identifier2,
        Title = "Do not put me on second place!",
        Description = "You already did it, didn't you?",
        IsCompleted = true,
        DueDate = DateTime.MaxValue,
        Priority = 101
    };

    public EfTodoRepositoryTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new(options, configuration);
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
        Assert.Equal(_todoEntity1.Id, todo.Id);
        Assert.Equal(_todoEntity1.Title, todo.Title);
        Assert.Equal(_todoEntity1.Description, todo.Description);
        Assert.Equal(_todoEntity1.IsCompleted, todo.IsCompleted);
        Assert.Equal(_todoEntity1.DueDate, todo.DueDate);
        Assert.Equal(_todoEntity1.Priority, todo.Priority);
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
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var todo = new Todo()
        {
            Id = Guid.NewGuid(),
            Title = "Third wheel",
            Description = "Don't even ask!",
            IsCompleted = true,
            DueDate = DateTime.MaxValue,
            Priority = 3
        };

        // Act
        await _repository.CreateAsync(todo, CancellationToken.None);

        // Assert
        var todos = await _repository.GetListAsync(CancellationToken.None);
        Assert.NotNull(todos);
        Assert.Equal(3, todos.Count());

        var newTodo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == todo.Id);
        Assert.NotNull(newTodo);
        Assert.Equal(todo.Id, newTodo.Id);
        Assert.Equal(todo.Title, newTodo.Title);
        Assert.Equal(todo.Description, newTodo.Description);
        Assert.Equal(todo.IsCompleted, newTodo.IsCompleted);
        Assert.Equal(todo.DueDate, newTodo.DueDate);
        Assert.Equal(todo.Priority, newTodo.Priority);
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
