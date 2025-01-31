
using AutoMapper;

using CrudPlay.Application.Interfaces;
using CrudPlay.Core.DTO;
using CrudPlay.Core.Exceptions;
using CrudPlay.Infrastructure.Interfaces;

using TodoEntity = CrudPlay.Core.Entities.Todo;
using TodoItem = CrudPlay.Core.Domain.Todo;

namespace CrudPlay.Infrastructure.Services;

public class TodoService(IMapper mapper, IRepository<TodoEntity> repository) : ITodoService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<TodoEntity> _repository = repository;

    public async Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        var todos = await _repository.GetAllAsync(cancellationToken);
        return (todos is null || !todos.Any()) ? throw new NotFoundException("No Todo items found") : _mapper.Map<IEnumerable<TodoItem>>(todos);
    }

    public async Task<TodoItem> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetByIdAsync(new Guid(id), cancellationToken);
        return todo is null ? throw new NotFoundException("No corresponding Todo item found") : _mapper.Map<TodoItem>(todo);
    }

    public async Task CreateAsync(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var todo = _mapper.Map<TodoEntity>(request);
        todo.CreatedAt = DateTime.UtcNow;
        todo.UpdatedAt = DateTime.UtcNow;

        await _repository.AddAsync(todo, cancellationToken);
    }

    public async Task UpdateAsync(string id, UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        var todo = (await _repository.GetByIdAsync(new Guid(id), cancellationToken) ?? throw new NotFoundException("No corresponding Todo item found"));
        if (todo is not null)
        {
            _mapper.Map(request, todo);

            await _repository.UpdateAsync(todo, cancellationToken);
        }

    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var identifier = new Guid(id);
        var todo = (await _repository.GetByIdAsync(identifier, cancellationToken) ?? throw new NotFoundException("No corresponding Todo item found"));

        await _repository.DeleteAsync(identifier, cancellationToken);
    }
}
