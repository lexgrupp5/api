using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class UnauthorizedException(string detail, string title = "Unauthorized")
    : ApiException(detail, title, StatusCodes.Status401Unauthorized) { }
