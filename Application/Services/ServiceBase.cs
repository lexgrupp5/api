using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Application.Interfaces;
using Domain.Exceptions;

namespace Application.Services;

public abstract class ServiceBase<T> : IServiceBase<T>
{
    protected static bool TryValidateDto<TDto>(TDto dto, out ICollection<ValidationResult> results)
    {
        results = [];

        if (dto is null)
            return false;

        var context = new ValidationContext(dto);
        return Validator.TryValidateObject(dto, context, results, true);
    }

    [DoesNotReturn]
    protected static void NotFound(
        string detail = "Entity not found",
        string title = "Not Found"
    ) => throw new NotFoundException(detail, title);

    [DoesNotReturn]
    protected static void BadRequest(string detail, string title = "Bad Request") =>
        throw new BadRequestException(detail, title);

    [DoesNotReturn]
    protected static void Unauthorized(string detail, string title = "Unauthorized") =>
        throw new UnauthorizedException(detail, title);

    [DoesNotReturn]
    protected static void Forbidden(string detail, string title = "Forbidden") =>
        throw new ForbiddenException(detail, title);
}
