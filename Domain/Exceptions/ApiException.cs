using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public abstract class ApiException(
    string detail = "An unexpected error occurred.",
    string title = "Internal Server Error",
    int statusCode = StatusCodes.Status500InternalServerError
    ) : Exception(detail)
{
    public int StatusCode { get; set; } = statusCode;
    public string Title { get; set; } = title;
    public string Detail { get; set; } = detail;
}
