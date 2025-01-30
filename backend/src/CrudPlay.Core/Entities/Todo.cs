using System.ComponentModel.DataAnnotations;

namespace CrudPlay.Core.Entities;

public class Todo : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsComplete { get; set; }

    public DateTime? DueDate { get; set; }

    public int Priority { get; set; }
}
