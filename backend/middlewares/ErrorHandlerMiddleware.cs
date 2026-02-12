using System.Net;
using System.Text.Json;
using backend.Errors;

namespace backend.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteErrorResponseAsync(context, ex);
        }
    }

    private static Task WriteErrorResponseAsync(HttpContext context, Exception exception)
    {
        // Si la respuesta ya comenzó, no podemos cambiar headers ni status code
        if (context.Response.HasStarted)
        {
            return Task.CompletedTask;
        }

        if (exception is HTTPError httpError)
        {
            var payload = JsonSerializer.Serialize(new { code = httpError.Status, message = httpError.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpError.Status;
            return context.Response.WriteAsync(payload);
        }

        var response = new { code = (int)HttpStatusCode.InternalServerError, message = "Unexpected error" };

        var fallbackPayload = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(fallbackPayload);
    }
}
