using CrudPlay.Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Infrastructure.Persistance;

public class EfTodoRepository<T> : IRepository<T> where T : class
{
    private readonly TodoDbContext _context;

    public EfTodoRepository(TodoDbContext context) => _context = context;

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) => await _context.Set<T>().ToListAsync(cancellationToken: cancellationToken);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await _context.Set<T>().FindAsync([id], cancellationToken: cancellationToken) ?? null;

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}