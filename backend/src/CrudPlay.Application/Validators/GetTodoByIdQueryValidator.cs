using CrudPlay.Application.Queries;
using CrudPlay.Application.Validators.Interfaces;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Validators;

public class GetTodoByIdQueryValidator : IGetTodoByIdQueryValidator
{
    public void ValidateOrThrowException(GetTodoByIdQuery query)
    {
        if (!Guid.TryParse(query.Id, out _))
        {
            throw new ApplicationValidatorException("Identifier is not a valid Guid");
        }
    }
}
