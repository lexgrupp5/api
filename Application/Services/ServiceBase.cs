using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using Domain.Exceptions;
using Infrastructure.Models;

namespace Application.Services;

public abstract class ServiceBase<T> : IServiceBase<T>
{
    /*
     *
     ****/
    protected static bool TryValidateDto<TDto>(TDto dto, out ICollection<ValidationResult> results)
    {
        results = [];
        if (dto is null)
            return false;

        var context = new ValidationContext(dto);
        return Validator.TryValidateObject(dto, context, results, true);
    }

    /*
     *
     ****/
    protected Expression<Func<T, bool>>? CreateSearchFilter(
        string searchString,
        params Expression<Func<T, string>>[] properties
    )
    {
        if (string.IsNullOrEmpty(searchString))
            return null;

        var lowerSearchString = searchString.ToLowerInvariant();
        var parameter = Expression.Parameter(typeof(T), "x");
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;

        Expression? searchExpressions = null;
        foreach (var property in properties)
        {
            var expression = Expression.Call(
                Expression.Property(parameter, ((MemberExpression)property.Body).Member.Name),
                containsMethod,
                Expression.Constant(lowerSearchString)
            );

            searchExpressions =
                searchExpressions == null
                    ? expression
                    : Expression.OrElse(searchExpressions, expression);
        }

        return Expression.Lambda<Func<T, bool>>(searchExpressions!, parameter);
    }

    /*
     *
     ****/
    protected static (ICollection<SortParams>?, PageParams?) ParseQueryParams(
        QueryParams? queryParams
    )
    {
        if (queryParams == null)
            return (null, null);

        return (
            queryParams.SortFields,
            new() { Page = queryParams.Page ?? 0, Size = queryParams.Limit ?? 0 }
        );
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
