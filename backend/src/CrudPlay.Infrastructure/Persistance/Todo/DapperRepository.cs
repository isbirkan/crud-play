
using System.Data;
using System.Linq.Expressions;

using CrudPlay.Infrastructure.Interfaces;
using CrudPlay.Infrastructure.Persistance.Extensions;

using Dapper;

namespace CrudPlay.Infrastructure.Persistance.Todo;

public class DapperRepository<T>(IDbConnection dbConnection) : IRepository<T> where T : class
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken) =>
        await _dbConnection.QueryAsync<T>("GetTodoList", commandType: CommandType.StoredProcedure);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbConnection.QueryFirstOrDefaultAsync<T>("GetTodoById", new { Id = id }, commandType: CommandType.StoredProcedure);

    public async Task<IEnumerable<T>> GetByPropertyAsync<TProperty>(
        Expression<Func<T, TProperty>> propertySelector,
        TProperty value,
        CancellationToken cancellationToken)
    {
        string propertyName = propertySelector.GetPropertyName();

        return await _dbConnection.QueryAsync<T>(
            "GetTodoByProperty",
            new { PropertyName = propertyName, PropertyValue = value },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken) =>
        await _dbConnection.ExecuteAsync("AddTodo", new DynamicParameters(entity), commandType: CommandType.StoredProcedure);

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken) =>
    await _dbConnection.ExecuteAsync("UpdateTodo", entity, commandType: CommandType.StoredProcedure);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbConnection.ExecuteAsync("DeleteTodo", new { Id = id }, commandType: CommandType.StoredProcedure);
}
