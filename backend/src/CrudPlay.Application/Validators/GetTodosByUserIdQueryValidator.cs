using CrudPlay.Application.Queries;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Validators;

public class GetTodosByUserIdQueryValidator : IGetTodosByUserIdQueryValidator
{
    public void ValidateOrThrowException(GetTodosByUserIdQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.UserId))
        {
            throw new ApplicationValidatorException("User identifier cannot be null or empty");
        }
    }
}
