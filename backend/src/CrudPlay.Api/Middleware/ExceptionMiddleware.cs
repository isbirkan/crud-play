using System.Net;
using System.Text.Json;

using CrudPlay.Api.Models;
using CrudPlay.Core.Exceptions;

namespace CrudPlay.Api.Middleware;

public class ExceptionMiddleware(
    ILogger<ExceptionMiddleware> logger,
    IWebHostEnvironment env)
    : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(ex, context);
        }
    }

    private Task HandleException(Exception exception, HttpContext context)
    {
        _logger.LogError($"An error has occurred: {exception}");

        var debugInfo = _env.IsDevelopment() ? exception.ToString() : null;
        return exception switch
        {
            ApplicationValidatorException applicationValidatorException =>
                WriteResponse(context, "Validation", applicationValidatorException.Message, debugInfo, HttpStatusCode.BadRequest),

            ArgumentException argumentException =>
                WriteResponse(context, "Argument", argumentException.Message, debugInfo, HttpStatusCode.BadRequest),

            ForbiddenException =>
                WriteResponse(context, "Forbidden", string.Empty, debugInfo, HttpStatusCode.Forbidden),

            NotFoundException notFoundException =>
                WriteResponse(context, "NotFound", notFoundException.Message, debugInfo, HttpStatusCode.NotFound),

            _ => HandleGenericException(context, exception, debugInfo)
        };
    }

    private static async Task WriteResponse(HttpContext context, string type, string message, string debugInfo, HttpStatusCode statusCode)
    {
        context.Response.Headers.Append("Content-Type", "application/json");
        context.Response.StatusCode = (int)statusCode;
        if (context.Response.HasStarted) return;

        var response = new ErrorResponse(type, message, debugInfo);
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private Task HandleGenericException(HttpContext context, Exception exception, string debugInfo)
    {
        _logger.LogError($"Unhandled exception: {exception.Message}", exception);

        return WriteResponse(context, "Unhandled", "An unexpected error occurred", debugInfo, HttpStatusCode.InternalServerError);
    }

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
