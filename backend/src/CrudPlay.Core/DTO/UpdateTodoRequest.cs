namespace CrudPlay.Core.DTO;

public record UpdateTodoRequest(string? Title, string? Description, bool? IsComplete, DateTime? DueDate, int? Priority);
