namespace CrudPlay.Core.DTO;

public record CreateTodoRequest(string Title, string Description, DateTime? DueDate, int Priority);
