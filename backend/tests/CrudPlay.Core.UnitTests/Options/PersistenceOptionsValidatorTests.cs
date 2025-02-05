using CrudPlay.Core.Options;

namespace CrudPlay.Core.UnitTests.Options;

public class PersistenceOptionsValidatorTests
{
    private readonly PersistenceOptionsValidator _validator = new();
    private readonly PersistenceOptions _options = new()
    {
        ConnectionString = "letsConnect",
        Implementation = ImplementationType.EntityFramework
    };

    [Fact]
    public void Validate_PersistenceOptionsNull_ShouldReturnValidationResultFail()
    {
        // Act
        var result = _validator.Validate("", null);

        // Assert
        Assert.True(result.Failed);
        Assert.Equal("Missing config values section - Persistence", result.FailureMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_PersistenceOptions_ConnectionStringNullOrEmptyOrWhitespace_ShouldReturnValidationResultFail(string? connectionString)
    {
        // Arrange
        _options.ConnectionString = connectionString;

        // Act
        var result = _validator.Validate("", _options);

        // Assert
        Assert.True(result.Failed);
        Assert.Equal("ConnectionString must be provided", result.FailureMessage);
    }
}
