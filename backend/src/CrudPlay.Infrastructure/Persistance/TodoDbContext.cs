using CrudPlay.Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Infrastructure.Persistance;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

    public DbSet<Todo> Todos { get; set; }
}
