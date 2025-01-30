using CrudPlay.Application.Queries;

namespace CrudPlay.Application.Validators.Interfaces;

public interface IGetTodoByIdQueryValidator
{
    void ValidateOrThrowException(GetTodoByIdQuery query);
}
