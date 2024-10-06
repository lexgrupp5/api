using System.Linq.Expressions;
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

    public DbContext Context => _db;

    /*
     *
     ****/
    public IQueryable<T> GetQuery(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    )
    {
        return _db.Set<T>()
            .AsQueryable()
            .ApplyFilters(filters)
            .ApplySorting(sorting)
            .ApplyPagination(paging);
    }

    public (IQueryable<T>, int) GetQueryWithTotalItemCount(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    )
    {
        var dbSet = _db.Set<T>();
        var totalItemCount = dbSet.Count();

        return (
            dbSet
                .AsQueryable()
                .ApplyFilters(filters)
                .ApplySorting(sorting)
                .ApplyPagination(paging),
            totalItemCount);
    }

    /*
     *
     ****/
    public IQueryable<T> GetQueryById(params object?[]? keyValues) =>
        _db.Set<T>().AsQueryable().Where(FindByIdFilter(keyValues));

    /*
     *
     ****/
    public async Task<T?> FindAsync(params object?[]? keyValues) =>
        await _db.Set<T>().FindAsync(keyValues);

    /*
     *
     ****/
    public async Task<bool> ExistsAsync(params object?[]? keyValues) =>
        await _db.Set<T>().AnyAsync(FindByIdFilter(keyValues));

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
    public async Task<EntityEntry<T>> AddAsync(T entity) => await _db.Set<T>().AddAsync(entity);

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
        return query.ApplyFilters(filters).ApplySorting(sorting).ApplyPagination(paging);
    }

    protected Expression<Func<T, bool>> FindByIdFilter(params object?[]? keyValues)
    {
        ArgumentNullException.ThrowIfNull(keyValues);

        var entityType = _db.Model.FindEntityType(typeof(T));
        var primaryKeys = entityType?.FindPrimaryKey();

        ArgumentNullException.ThrowIfNull(primaryKeys);

        if (primaryKeys.Properties.Count != keyValues.Length)
            throw new ArgumentException("keyValues count does not match primaryKeys");

        var parameter = Expression.Parameter(typeof(T), nameof(T));
        var body = primaryKeys
            .Properties.Select(
                (p, i) =>
                    Expression.Equal(
                        Expression.Property(parameter, p.Name),
                        Expression.Constant(keyValues[i])
                    )
            )
            .Aggregate(Expression.AndAlso);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
