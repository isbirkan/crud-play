using CrudPlay.Application.Commands;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Validators;

public class UpdateTodoCommandValidator : IUpdateTodoCommandValidator
{
    public void ValidateOrThrowException(UpdateTodoCommand command)
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
