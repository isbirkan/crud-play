namespace CrudPlay.Core.Domain;

public class Todo
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsComplete { get; set; }

    public DateTime? DueDate { get; set; }

    public int Priority { get; set; }
}
