using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T>(AppDbContext context) : IRepositoryBase<T>
    where T : class
{
    protected AppDbContext _db = context;

    /*
     *
     ****/
    public IQueryable<T> GetQuery(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        ICollection<SortParams>? sorting = null,
        PageParams? paging = null,
        IEnumerable<Expression<Func<T, object>>>? includes = null
    )
    {
        var query = _db.Set<T>().AsQueryable();

        if (filters != null)
            query = ApplyFilters(query, filters);

        if (includes != null)
            query = ApplyIncludes(query, includes);

        if (sorting != null)
            query = ApplySorting(query, sorting);

        if (paging != null)
            query = ApplyPagination(query, paging);

        return query;
    }

    /*
     *
     ****/
    public IEnumerable<T> GetAll() => [.. _db.Set<T>()];

    /*
     *
     ****/
    public async Task<IEnumerable<T>> GetAllAsync() => await _db.Set<T>().ToListAsync();

    /*
     *
     ****/
    public IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression) =>
        _db.Set<T>().Where(expression);

    /*
     *
     ****/
    public async Task<EntityEntry<T>> CreateAsync(T entity) => await _db.Set<T>().AddAsync(entity);

    /*
     *
     ****/
    public EntityEntry<T> Update(T entity) => _db.Set<T>().Update(entity);

    /*
     *
     ****/
    public EntityEntry<T> Delete(T entity) => _db.Set<T>().Remove(entity);

    /*
     *
     ****/
    protected IQueryable<T> ApplyFilters(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, bool>>>? filters
    ) => filters?.Aggregate(query, (cur, filter) => cur.Where(filter)) ?? query;

    /*
     *
     ****/
    protected IQueryable<T> ApplyIncludes(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, object>>>? includes
    ) => includes?.Aggregate(query, (cur, include) => cur.Include(include)) ?? query;

    /*
     *
     ****/
    protected IQueryable<T> ApplySorting(
        IQueryable<T> query,
        ICollection<SortParams>? sortingOptions
    )
    {
        if (sortingOptions is null || sortingOptions.Count == 0)
            return query;

        foreach (var sort in sortingOptions)
        {
            var propInfo = typeof(T).GetProperty(
                sort.Field,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );
            if (propInfo == null)
                continue;

            var parameter = Expression.Parameter(typeof(T), propInfo.Name);
            var propertyAccess = Expression.MakeMemberAccess(parameter, propInfo);
            var lambda =
                (Expression<Func<T, object>>)
                    Expression.Lambda(
                        typeof(Func<,>).MakeGenericType(typeof(T), typeof(object)),
                        propertyAccess,
                        parameter
                    );
            query = query.SmartOrderBy(lambda, sort.Descending);
        }

        return query;
    }

    /*
     *
     ****/
    protected IQueryable<T> ApplyPagination(IQueryable<T> query, PageParams? pagination)
    {
        if (pagination == null)
            return query;

        pagination.Page = Math.Max(pagination.Page, 1);
        pagination.Size = pagination.Size > 0 ? pagination.Size : 10;

        return query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size);
    }
}
