using CrudPlay.Application.Commands;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Validators;

public class CreateTodoCommandValidator : ICreateTodoCommandValidator
{
    public void ValidateOrThrowException(CreateTodoCommand command)
    {
        if (string.IsNullOrWhiteSpace(command?.Request.Title))
        {
            throw new ApplicationValidatorException("Todo Title cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(command?.Request.Description))
        {
            throw new ApplicationValidatorException("Todo Description cannot be null or empty");
        }
    }
}
