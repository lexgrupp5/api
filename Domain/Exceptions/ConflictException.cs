using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class ConflictException(string detail, string title = "Conflict")
    : ApiException(detail, title, StatusCodes.Status409Conflict) { }
