namespace CrudPlay.Core.Domain;

public record Todo(string Title, string Description, bool IsCompleted, DateTime? DueDate, int Priority);
