namespace CrudPlay.Core.Domain;

public record Todo(string Id, string Title, string Description, bool IsCompleted, DateTime? DueDate, int Priority);
