using CrudPlay.Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Infrastructure.Persistance;

internal class TodoRepository<T> : IRepository<T> where T : class
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context) => _context = context;

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken) => await _context.Set<T>().ToListAsync();

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken) => await _context.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}