using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Middlewares;

public class JsonValidMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentType == null || !context.Request.ContentType.Contains("json"))
        {
            await _next(context);
            return;
        }

        context.Request.EnableBuffering();

        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        try
        {
            _ = JsonDocument.Parse(body);
        }
        catch (JsonException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    new ProblemDetails()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Bad Request",
                        Detail = "Invalid JSON",
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Instance = context.Request.Path
                    }
                )
            );
            return;
        }

        await _next(context);
    }
}
