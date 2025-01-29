using CrudPlay.Application.Models;

namespace CrudPlay.Application.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken);
}
