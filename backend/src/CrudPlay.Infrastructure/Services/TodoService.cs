
using AutoMapper;

using CrudPlay.Application.Interfaces;
using CrudPlay.Core.DTO;
using CrudPlay.Core.Exceptions;
using CrudPlay.Infrastructure.Interfaces;

using Microsoft.Extensions.Logging;

using TodoEntity = CrudPlay.Core.Entities.Todo;
using TodoItem = CrudPlay.Core.Domain.Todo;

namespace CrudPlay.Infrastructure.Services;

internal class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<TodoEntity> _repository;

    public TodoService(ILogger<TodoService> logger, IMapper mapper, IRepository<TodoEntity> repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var todos = await _repository.GetAllAsync(cancellationToken);
            return todos is null ? throw new NotFoundException("No Todo items found") : _mapper.Map<IEnumerable<TodoItem>>(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving the Todos list: {ex}");
            throw new Exception("An error occurred while retrieving the Todos list.");
        }
    }

    public async Task<TodoItem> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var todo = await _repository.GetByIdAsync(new Guid(id), cancellationToken);

            return todo is null ? throw new NotFoundException("No corresponding Todo item found") : _mapper.Map<TodoItem>(todo);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving the Todo item: {ex}");
            throw new Exception("An error occurred while retrieving the Todo item.");
        }

    }

    public async Task CreateAsync(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var todo = _mapper.Map<TodoEntity>(request);
            todo.CreatedAt = DateTime.UtcNow;
            todo.UpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(todo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while creating the Todo item: {ex}");
            throw new Exception("An error occurred while creating the Todo item.");
        }
    }

    public async Task UpdateAsync(string id, UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var todo = await _repository.GetByIdAsync(new Guid(id), cancellationToken) ?? throw new NotFoundException("No corresponding Todo item found");

            // Map when different
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while updating the Todo item: {ex}");
            throw new Exception("An error occurred while updating the Todo item.");
        }
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var identifier = new Guid(id);
            var todo = await _repository.GetByIdAsync(identifier, cancellationToken) ?? throw new NotFoundException("No corresponding Todo item found");

            await _repository.DeleteAsync(identifier, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while deleting the Todo item: {ex}");
            throw new Exception("An error occurred while deleting the Todo item.");
        }
    }
}
