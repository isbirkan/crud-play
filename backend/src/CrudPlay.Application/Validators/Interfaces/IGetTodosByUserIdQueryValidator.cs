using CrudPlay.Application.Queries;

namespace CrudPlay.Application.Validators.Interfaces;

public interface IGetTodosByUserIdQueryValidator
{
    void ValidateOrThrowException(GetTodosByUserIdQuery query);
}
