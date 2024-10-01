using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    protected AppDbContext _db;

    public RepositoryBase(AppDbContext context)
    {
        _db = context;
    }

    /*
     *
     ****/
    public IQueryable<T> GetQuery(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    ) => BuildQuery(_db.Set<T>().AsQueryable(), filters, sorting, paging);

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
    protected IQueryable<T> BuildQuery(
        IQueryable<T> query,
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    )
    {
        if (filters != null)
            query = ApplyFilters(query, filters);

        if (sorting != null)
            query = ApplySorting(query, sorting);

        if (paging != null)
            query = ApplyPagination(query, paging);

        return query;
    }

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
    protected IQueryable<T> ApplySorting(IQueryable<T> query, IEnumerable<SortParams>? sorting)
    {
        if (sorting is null || !sorting.Any())
            return query;

        foreach (var sort in sorting)
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
