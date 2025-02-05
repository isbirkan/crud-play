using CrudPlay.Application.Queries;
using CrudPlay.Application.Validators;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.UnitTests.Validators;

public class GetTodosByUserIdQueryValidatorTests
{
    private readonly GetTodosByUserIdQueryValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateOrThrowException_IdentifierNullOrEmptyOrWhitespace_ShouldThrowException(string? identifier)
    {
        // Arrange
        var query = new GetTodosByUserIdQuery(identifier);

        // Act
        void action() => _validator.ValidateOrThrowException(query);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("User identifier cannot be null or empty", exception.Message);
    }
}
