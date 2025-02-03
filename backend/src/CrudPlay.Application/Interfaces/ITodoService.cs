using CrudPlay.Core.DTO;

using TodoItem = CrudPlay.Core.Domain.TodoModel;

namespace CrudPlay.Application.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken);

    Task<TodoItem> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task CreateAsync(CreateTodoRequest request, CancellationToken cancellationToken);

    Task UpdateAsync(string id, UpdateTodoRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
