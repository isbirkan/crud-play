using System.Linq.Expressions;

using CrudPlay.Core.Entities;
using CrudPlay.Infrastructure.Persistance.Extensions;

namespace CrudPlay.Infrastructure.UnitTests.Persistence.Extensions;

public class ExpressionExtensionsTests
{
    [Fact]
    public void GetPropertyName_ShouldReturnCorrectPropertyName_ForString()
    {
        // Arrange
        Expression<Func<TodoEntity, string>> expression = x => x.Title;

        // Act
        var propertyName = expression.GetPropertyName();

        // Assert
        Assert.Equal("Title", propertyName);
    }

    [Fact]
    public void GetPropertyName_ShouldReturnCorrectPropertyName_ForInt()
    {
        // Arrange
        Expression<Func<TodoEntity, int>> expression = x => x.Priority;

        // Act
        var propertyName = expression.GetPropertyName();

        // Assert
        Assert.Equal("Priority", propertyName);
    }

    [Fact]
    public void GetPropertyName_ShouldReturnCorrectPropertyName_ForBool()
    {
        // Arrange
        Expression<Func<TodoEntity, bool>> expression = x => x.IsCompleted;

        // Act
        var propertyName = expression.GetPropertyName();

        // Assert
        Assert.Equal("IsCompleted", propertyName);
    }

    [Fact]
    public void GetPropertyName_ShouldReturnCorrectPropertyName_ForDateTime()
    {
        // Arrange
        Expression<Func<TodoEntity, DateTime?>> expression = x => x.DueDate;

        // Act
        var propertyName = expression.GetPropertyName();

        // Assert
        Assert.Equal("DueDate", propertyName);
    }

    [Fact]
    public void GetPropertyName_ShouldThrowArgumentException_ForInvalidExpression()
    {
        // Arrange
        Expression<Func<TodoEntity, object>> expression = x => x.Title.Length;

        // Act
        void action() => expression.GetPropertyName();

        // Assert
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Invalid property expression", exception.Message);
    }
}
