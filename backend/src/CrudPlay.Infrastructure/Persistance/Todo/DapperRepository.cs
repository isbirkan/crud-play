
using System.Data;

using CrudPlay.Infrastructure.Interfaces;

using Dapper;

namespace CrudPlay.Infrastructure.Persistance.Todo;

public class DapperRepository<T>(IDbConnection dbConnection) : IRepository<T> where T : class
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken) =>
        await _dbConnection.QueryAsync<T>("GetTodoList", commandType: CommandType.StoredProcedure);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbConnection.QueryFirstOrDefaultAsync<T>("GetTodoById", new { Id = id }, commandType: CommandType.StoredProcedure);

    public async Task CreateAsync(T entity, CancellationToken cancellationToken) =>
        await _dbConnection.ExecuteAsync("AddTodo", new DynamicParameters(entity), commandType: CommandType.StoredProcedure);

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken) =>
    await _dbConnection.ExecuteAsync("UpdateTodo", entity, commandType: CommandType.StoredProcedure);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbConnection.ExecuteAsync("DeleteTodo", new { Id = id }, commandType: CommandType.StoredProcedure);
}
