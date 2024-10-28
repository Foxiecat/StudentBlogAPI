using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StudentBlogAPI.Middleware;

public class GlobalExceptionHandling(ILogger<GlobalExceptionHandling> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Could not process a request on Machine {MachineName}. TraceId:{TraceId}",
            Environment.MachineName, httpContext.TraceIdentifier);

        // Mapping
        // statuscode and title
        (int statusCode, string? title) = MapException(exception);
        
        await Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>()
            {
                { "traceId", httpContext.TraceIdentifier }
            }
        ).ExecuteAsync(httpContext);

        return true; // we stop the pipeline
    }

    private (int statusCodes, string title) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => (StatusCodes.Status400BadRequest, "You made a mistake!"),
            BadHttpRequestException => (StatusCodes.Status400BadRequest, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }
}