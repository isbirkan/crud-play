using CrudPlay.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Infrastructure.Persistance.Todo;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoEntity> Todos { get; set; }
}
