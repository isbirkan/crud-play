using System.Net;
using System.Text.Json;

using CrudPlay.Api.Middleware;
using CrudPlay.Api.Models;
using CrudPlay.Core.Exceptions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using NSubstitute;

namespace CrudPlay.Api.UnitTests;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task ExceptionMiddleware_NoException_ShouldNotHandle()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();

        const string expectedOutput = "Request handed over to next request delegate";


        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            innerHttpContext.Response.WriteAsync(expectedOutput);
            return Task.CompletedTask;
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();

        Assert.Equal(expectedOutput, body);

        logger.DidNotReceiveWithAnyArgs().Log(default, default, default, default, default);
    }

    [Fact]
    public async Task ExceptionMiddleware_GenericException_EnvNotDevelopment_ShouldLogAndReturnType()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("NotDevelopment");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new Exception("Generic exception message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.InternalServerError, defaultContext.Response.StatusCode);
        Assert.Equal("Unhandled", errorResponse.Type);
        Assert.Equal("An unexpected error occurred", errorResponse.Message);
        Assert.Null(errorResponse.DebugInfo);
    }

    [Fact]
    public async Task ExceptionMiddleware_GenericException_EnvDevelopment_ShouldLogAndReturnTypeAndDebugInfo()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new Exception("Generic exception message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.InternalServerError, defaultContext.Response.StatusCode);
        Assert.Equal("Unhandled", errorResponse.Type);
        Assert.Equal("An unexpected error occurred", errorResponse.Message);
        Assert.NotNull(errorResponse.DebugInfo);

        logger.ReceivedWithAnyArgs().Log(default, default, default, default, default);
    }

    [Fact]
    public async Task ExceptionMiddleware_ApplicationValidatorException_EnvNotDevelopment_ShouldLogAndReturnTypeAndMessage()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("NotDevelopment");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new ApplicationValidatorException("ApplicationValidatorException message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.BadRequest, defaultContext.Response.StatusCode);
        Assert.Equal("Validation", errorResponse.Type);
        Assert.Equal("ApplicationValidatorException message", errorResponse.Message);
        Assert.Null(errorResponse.DebugInfo);

        logger.ReceivedWithAnyArgs().Log(default, default, default, default, default);
    }

    [Fact]
    public async Task ExceptionMiddleware_ApplicationValidatorException_EnvDevelopment_ShouldLogAndReturnTypeAndMessageAndDebugInfo()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new ApplicationValidatorException("ApplicationValidatorException message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.BadRequest, defaultContext.Response.StatusCode);
        Assert.Equal("Validation", errorResponse.Type);
        Assert.Equal("ApplicationValidatorException message", errorResponse.Message);
        Assert.NotNull(errorResponse.DebugInfo);

        logger.ReceivedWithAnyArgs().Log(default, default, default, default, default);
    }

    [Fact]
    public async Task ExceptionMiddleware_ArgumentExceptionException_EnvNotDevelopment_ShouldLogAndReturnTypeAndMessage()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("NotDevelopment");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new ArgumentException("ArgumentException message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.BadRequest, defaultContext.Response.StatusCode);
        Assert.Equal("Argument", errorResponse.Type);
        Assert.Equal("ArgumentException message", errorResponse.Message);
        Assert.Null(errorResponse.DebugInfo);

        logger.ReceivedWithAnyArgs().Log(default, default, default, default, default);
    }

    [Fact]
    public async Task ExceptionMiddleware_ArgumentExceptionException_EnvDevelopment_ShouldLogAndReturnTypeAndMessageAndDebugInfo()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new ArgumentException("ArgumentException message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.BadRequest, defaultContext.Response.StatusCode);
        Assert.Equal("Argument", errorResponse.Type);
        Assert.Equal("ArgumentException message", errorResponse.Message);
        Assert.NotNull(errorResponse.DebugInfo);

        logger.ReceivedWithAnyArgs().Log(default, default, default, default, default);
    }

    [Fact]
    public async Task ExceptionMiddleware_NotFoundException_EnvNotDevelopment_ShouldLogAndReturnType()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("NotDevelopment");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new NotFoundException("NotFoundException message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.NotFound, defaultContext.Response.StatusCode);
        Assert.Equal("NotFound", errorResponse.Type);
        Assert.Equal("NotFoundException message", errorResponse.Message);
        Assert.Null(errorResponse.DebugInfo);
    }

    [Fact]
    public async Task ExceptionMiddleware_NotFoundException_EnvDevelopment_ShouldLogAndReturnTypeAndDebugInfo()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new NotFoundException("NotFoundException message");
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.NotFound, defaultContext.Response.StatusCode);
        Assert.Equal("NotFound", errorResponse.Type);
        Assert.Equal("NotFoundException message", errorResponse.Message);
        Assert.NotNull(errorResponse.DebugInfo);

        logger.ReceivedWithAnyArgs().Log(default, default, default, default, default);
    }

    [Fact]
    public async Task ExceptionMiddleware_ForbiddenException_EnvNotDevelopment_ShouldLogAndReturnType()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("NotDevelopment");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new ForbiddenException();
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.Forbidden, defaultContext.Response.StatusCode);
        Assert.Equal("Forbidden", errorResponse.Type);
        Assert.Empty(errorResponse.Message);
        Assert.Null(errorResponse.DebugInfo);
    }

    [Fact]
    public async Task ExceptionMiddleware_ForbiddenException_EnvDevelopment_ShouldLogAndReturnTypeAndDebugInfo()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        var defaultContext = new DefaultHttpContext();
        defaultContext.Response.Body = new MemoryStream();
        defaultContext.Request.Path = "/";

        // Act
        var middlewareInstance = new ExceptionMiddleware(next: (innerHttpContext) =>
        {
            throw new ForbiddenException();
        }, logger, env);

        await middlewareInstance.InvokeAsync(defaultContext);

        // Assert
        defaultContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(defaultContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(body);

        Assert.Equal((int)HttpStatusCode.Forbidden, defaultContext.Response.StatusCode);
        Assert.Equal("Forbidden", errorResponse.Type);
        Assert.Empty(errorResponse.Message);
        Assert.NotNull(errorResponse.DebugInfo);

        logger.ReceivedWithAnyArgs().Log(default, default, default, default, default);
    }
}
