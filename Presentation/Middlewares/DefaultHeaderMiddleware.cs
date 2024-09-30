namespace Presentation.Middlewares;

public class DefaultHeaderMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey("Content-type"))
                context.Response.Headers.ContentType = "application/json";
            return Task.CompletedTask;
        });
        await _next(context);
    }
}
