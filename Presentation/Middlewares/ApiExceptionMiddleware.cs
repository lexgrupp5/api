using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace Presentation.Middlewares;

public class ApiExceptionMiddleware(
    RequestDelegate next,
    ProblemDetailsFactory problemDetailsFactory
)
{
    private readonly RequestDelegate _next = next;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, ApiException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.StatusCode;

        await context.Response.WriteAsync(
            JsonConvert.SerializeObject(
                _problemDetailsFactory.CreateProblemDetails(
                    context,
                    exception.StatusCode,
                    exception.Title,
                    detail: exception.Message,
                    instance: context.Request.Path
                )
            )
        );
        
        return;
    }
}
