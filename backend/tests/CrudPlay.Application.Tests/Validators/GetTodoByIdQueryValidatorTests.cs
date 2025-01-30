using CrudPlay.Application.Queries;
using CrudPlay.Application.Validators;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Tests.Validators;

public class GetTodoByIdQueryValidatorTests
{
    private readonly GetTodoByIdQueryValidator _validator = new();

    [Fact]
    public void ValidateOrThrowException_CommandNull_ShouldThrowException()
    {
        // Act
        void action() => _validator.ValidateOrThrowException(null);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("Request object cannot be null", exception.Message);
    }

    [Fact]
    public void ValidateOrThrowException_IdentifierNotValid_ShouldThrowException()
    {
        // Arrange
        var query = new GetTodoByIdQuery("to-guid-or-not-to-guid");

        // Act
        void action() => _validator.ValidateOrThrowException(query);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("Identifier is not a valid Guid", exception.Message);
    }
}
