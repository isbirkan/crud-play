using System.Linq.Expressions;

using CrudPlay.Infrastructure.Interfaces;
using CrudPlay.Infrastructure.Persistance.Extensions;

using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Infrastructure.Persistance.Todo;

public class EfTodoRepository<T>(TodoDbContext context) : IRepository<T> where T : class
{
    private readonly TodoDbContext _context = context;

    public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken) => await _context.Set<T>().ToListAsync(cancellationToken: cancellationToken);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await _context.Set<T>().FindAsync([id], cancellationToken: cancellationToken) ?? null;

    public async Task<IEnumerable<T>> GetByPropertyAsync<TProperty>(
        Expression<Func<T, TProperty>> propertySelector,
        TProperty value,
        CancellationToken cancellationToken)
    {
        return await _context.Set<T>()
            .Where(entity => EF.Property<TProperty>(entity, propertySelector.GetPropertyName())!.Equals(value))
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
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