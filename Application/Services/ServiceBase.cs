using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.Services;

public abstract class ServiceBase<TEntity, TDto> : IServiceBase<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IDataCoordinator _data;
    protected readonly IMapper _mapper;

    protected ServiceBase(IDataCoordinator dataCoordinator, IMapper mapper)
    {
        _data = dataCoordinator;
        _mapper = mapper;
    }

    /*
     *
     ****/
    public virtual async Task<TDto?> FindAsync(params object?[]? keyValues) =>
        _mapper.Map<TDto>(await _data.Set<TEntity>().FindAsync(keyValues));

    /*
     *
     ****/
    public virtual async Task<TDto?> CreateAsync<TCreateDto>(TCreateDto createDto)
    {
        ArgumentNullException.ThrowIfNull(createDto);

        var newEntity = _mapper.Map<TEntity>(createDto);
        await _data.Set<TEntity>().AddAsync(newEntity);
        await _data.CompleteAsync();

        return _mapper.Map<TDto>(newEntity);
    }

    /*
     *
     ****/
    public virtual async Task<TDto> UpdateAsync<TUpdateDto>(object id, TUpdateDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(updateDto);

        var idValue = typeof(TUpdateDto).GetProperty("Id")?.GetValue(updateDto);
        if (idValue == null)
            throw new ArgumentException($"{nameof(TUpdateDto)} does not contain an Id property.");

        if (id != idValue)
            BadRequest("Id missmatch");

        var currentEntity = await _data.Set<TEntity>().FindAsync(idValue);
        if (currentEntity == null)
            NotFound();

        _mapper.Map(updateDto, currentEntity);
        await _data.CompleteAsync();

        return _mapper.Map<TDto>(currentEntity);
    }

    /*
     *
     ****/
    public virtual async Task<bool> DeleteAsync(object id, TDto entityDto) =>
        await DeleteAsync<TDto>(id, entityDto);

    /*
     *
     ****/
    public virtual async Task<bool> DeleteAsync<T>(object id, T entityDto)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idValue = typeof(T).GetProperty("Id")?.GetValue(entityDto);
        if (idValue == null)
            throw new ArgumentException($"{nameof(T)} does not contain an Id property.");

        if (id != idValue)
            BadRequest("Id missmatch");

        var entity = await _data.Set<TEntity>().FindAsync(idValue);
        if (entity == null)
            NotFound();

        return await DeleteAsync(entity);
    }

    /* PROTECTED HELPERS
     ************************************************************************/

    /*
     *
     ****/
    protected virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _data.Set<TEntity>().Remove(entity);
        return await _data.CompleteAsync() > 0;
    }

    /*
     *
     ****/
    protected static bool TryValidateDto(TDto dto, out ICollection<ValidationResult> results)
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
    protected virtual Expression<Func<TEntity, bool>>? CreateSearchFilter(
        string searchString,
        params Expression<Func<TEntity, string>>[] properties
    )
    {
        if (string.IsNullOrEmpty(searchString))
            return null;

        var lowerSearchString = searchString.ToLowerInvariant();
        var parameter = Expression.Parameter(typeof(TEntity), "x");
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

        return Expression.Lambda<Func<TEntity, bool>>(searchExpressions!, parameter);
    }

    /*
     *
     ****/
    protected virtual (ICollection<SortParams>?, PageParams?) ParseQueryParams(
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

    /* Exceptions
     ************************************************************************/
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
