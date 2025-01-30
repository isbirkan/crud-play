using System.Net;
using System.Text.Json;

using CrudPlay.Api.Models;
using CrudPlay.Core.Exceptions;

using Microsoft.AspNetCore.Diagnostics;

namespace CrudPlay.Api.Middleware;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IWebHostEnvironment env)
    : IExceptionHandler
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(ex, context);
        }
    }

    private Task HandleException(Exception exception, HttpContext context)
    {
        _logger.LogError($"An error has occurred: {exception}");

        var debugInfo = _env.IsDevelopment() ? exception.ToString() : string.Empty;
        switch (exception)
        {
            case ApplicationValidatorException applicationValidatorEx:
                return WriteResponse(context, "Validation", applicationValidatorEx.Message, debugInfo, HttpStatusCode.BadRequest);
            case ForbiddenException:
                return WriteResponse(context, "Forbidden", string.Empty, debugInfo, HttpStatusCode.Forbidden);
            case NotFoundException:
                return WriteResponse(context, "NotFound", string.Empty, debugInfo, HttpStatusCode.NotFound);
            default:
                return WriteResponse(context, "Unhandled", string.Empty, debugInfo, HttpStatusCode.InternalServerError);
        }
    }

    private static async Task WriteResponse(HttpContext context, string type, string message, string debugInfo, HttpStatusCode statusCode)
    {
        context.Response.Headers.Append("Content-Type", "application/json");
        context.Response.StatusCode = (int)statusCode;
        if (context.Response.HasStarted) return;

        var response = new ErrorResponse(type, message, debugInfo);
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
