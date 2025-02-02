using CrudPlay.Application.Commands;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Validators;

public class DeleteTodoCommandValidator : IDeleteTodoCommandValidator
{
    public void ValidateOrThrowException(DeleteTodoCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Id))
        {
            throw new ApplicationValidatorException("Identifier must not be null");
        }

        if (!Guid.TryParse(command.Id, out _))
        {
            throw new ApplicationValidatorException("Identifier is not a valid Guid");
        }
    }
}
