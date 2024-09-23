using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class NotFoundException(string detail, string title = "Not Found")
    : ApiException(detail, title, StatusCodes.Status404NotFound) { }
