using System.Linq.Expressions;

namespace CrudPlay.Infrastructure.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<T>> GetByPropertyAsync<TProperty>(
        Expression<Func<T, TProperty>> propertySelector,
        TProperty value,
        CancellationToken cancellationToken);

    Task CreateAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
