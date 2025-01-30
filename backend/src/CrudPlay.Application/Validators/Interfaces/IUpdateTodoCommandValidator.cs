using CrudPlay.Application.Commands;

namespace CrudPlay.Application.Validators.Interfaces;

public interface IUpdateTodoCommandValidator
{
    void ValidateOrThrowException(UpdateTodoCommand command);
}
