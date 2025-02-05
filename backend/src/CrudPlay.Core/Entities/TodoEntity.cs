using System.ComponentModel.DataAnnotations;

using CrudPlay.Core.Identity;

namespace CrudPlay.Core.Entities;

public class TodoEntity : BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }

    public int Priority { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
