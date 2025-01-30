using CrudPlay.Application.Commands;

namespace CrudPlay.Application.Validators.Interfaces;

public interface ICreateTodoCommandValidator
{
    void ValidateOrThrowException(CreateTodoCommand command);
}
