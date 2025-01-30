using CrudPlay.Application.Commands;

namespace CrudPlay.Application.Validators.Interfaces;

public interface IDeleteTodoCommandValidator
{
    void ValidateOrThrowException(DeleteTodoCommand command);
}
