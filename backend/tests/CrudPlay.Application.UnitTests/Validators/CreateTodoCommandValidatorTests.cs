using CrudPlay.Application.Commands;
using CrudPlay.Application.Validators;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Application.Tests.Validators;

public class CreateTodoCommandValidatorTests
{
    private readonly CreateTodoCommandValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateOrThrowException_TitleNullOrEmptyOrWhitespace_ShouldThrowException(string title)
    {
        // Arrange
        var command = new CreateTodoCommand(new(title, "Super cute description", null, 1));

        // Act
        void action() => _validator.ValidateOrThrowException(command);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("Todo Title cannot be null", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateOrThrowException_DescriptionNullOrEmptyOrWhitespace_ShouldThrowException(string description)
    {
        // Arrange
        var command = new CreateTodoCommand(new("Am I a Title now?", description, null, 1));

        // Act
        void action() => _validator.ValidateOrThrowException(command);

        // Assert
        var exception = Assert.Throws<ApplicationValidatorException>(action);
        Assert.Equal("Todo Description cannot be null", exception.Message);
    }
}
