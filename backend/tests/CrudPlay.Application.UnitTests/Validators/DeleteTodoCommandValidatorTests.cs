using CrudPlay.Application.Commands;
using CrudPlay.Application.Validators;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Tests.Validators;

public class DeleteTodoCommandValidatorTests
{
    private readonly DeleteTodoCommandValidator _validator = new();

    [Fact]
    public void ValidateOrThrowException_CommandNull_ShouldThrowException()
    {
        // Act
        void action() => _validator.ValidateOrThrowException(null);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("Request object cannot be null", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateOrThrowException_IdentifierNullOrEmptyOrWhitespace_ShouldThrowException(string identifier)
    {
        // Arrange
        var command = new DeleteTodoCommand(identifier);

        // Act
        void action() => _validator.ValidateOrThrowException(command);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("Identifier must not be null", exception.Message);
    }

    [Fact]
    public void ValidateOrThrowException_IdentifierNotValid_ShouldThrowException()
    {
        // Arrange
        var command = new DeleteTodoCommand("the-answer-is-42");

        // Act
        void action() => _validator.ValidateOrThrowException(command);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("Identifier is not a valid Guid", exception.Message);
    }
}
