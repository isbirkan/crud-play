namespace CrudPlay.Core.Domain;

public record TodoModel(string Id, string Title, string Description, bool IsCompleted, DateTime? DueDate, int Priority);
