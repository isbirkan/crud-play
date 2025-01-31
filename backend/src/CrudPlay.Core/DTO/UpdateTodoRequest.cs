namespace CrudPlay.Core.DTO;

public record UpdateTodoRequest(string? Title, string? Description, bool? IsCompleted, DateTime? DueDate, int? Priority);
