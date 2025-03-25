using System.Diagnostics;

namespace WebApp.Logging;


public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestLoggingMiddleware> logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestPath = context.Request.Path.ToString().ToLower();

        // Ignore logging for JS and CSS files
        if (requestPath.EndsWith(".js") || requestPath.EndsWith(".css"))
        {
            await next(context);  // Skip logging for JS/CSS files
            return;
        }

        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Incoming Request: {Method} {Path}", context.Request.Method, context.Request.Path);

        await next(context);

        stopwatch.Stop();

        logger.LogInformation("Response: {StatusCode} in {ElapsedMilliseconds}ms", context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}