using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class ForbiddenException(string detail, string title = "Forbidden")
    : ApiException(detail, title, StatusCodes.Status403Forbidden) { }
