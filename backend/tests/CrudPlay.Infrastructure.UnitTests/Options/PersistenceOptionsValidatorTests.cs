using CrudPlay.Infrastructure.Options;

namespace CrudPlay.Infrastructure.UnitTests.Options;

public class PersistenceOptionsValidatorTests
{
    private readonly PersistenceOptionsValidator _validator = new();
    private readonly PersistenceOptions _options = new()
    {
        ConnectionString = "letsConnect",
        Implementation = "thisistheway"
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
    public void Validate_PersistenceOptions_ConnectionStringNullOrEmptyOrWhitespace_ShouldReturnValidationResultFail(string connectionString)
    {
        // Arrange
        _options.ConnectionString = connectionString;

        // Act
        var result = _validator.Validate("", _options);

        // Assert
        Assert.True(result.Failed);
        Assert.Equal("ConnectionString must be provided", result.FailureMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_PersistenceOptions_ImplementationNullOrEmptyOrWhitespace_ShouldReturnValidationResultFail(string implementation)
    {
        // Arrange
        _options.Implementation = implementation;

        // Act
        var result = _validator.Validate("", _options);

        // Assert
        Assert.True(result.Failed);
        Assert.Equal("Implementation must be provided", result.FailureMessage);
    }

    [Fact]
    public void Validate_PersistenceOptions_ImplementationInvalid_ShouldReturnValidationResultFail()
    {
        // Arrange
        _options.Implementation = "NobodyWillKnow";

        // Act
        var result = _validator.Validate("", _options);

        // Assert
        Assert.True(result.Failed);
        Assert.Equal("Implementation 'NobodyWillKnow' is not valid. Supported values are: EntityFramework, Dapper", result.FailureMessage);
    }

    [Theory]
    [InlineData("EntityFramework")]
    [InlineData("Dapper")]
    public void Validate_PersistenceOptions_ImplementationValid_ShouldReturnValidationResultSuccess(string implementation)
    {
        // Arrange
        _options.Implementation = implementation;

        // Act
        var result = _validator.Validate("", _options);

        // Assert
        Assert.True(result.Succeeded);
    }
}
