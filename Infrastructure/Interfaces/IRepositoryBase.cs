using System.Linq.Expressions;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Interfaces;

public interface IRepositoryBase<T>
    where T : class
{
    DbContext Context { get; }

    IQueryable<T> GetQuery(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    );

    (IQueryable<T>, int) GetQueryWithTotalItemCount(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    );

    IQueryable<T> GetQueryById(params object?[]? keyValues);

    Task<bool> ExistsAsync(params object?[]? keyValues);

    Task<T?> FindAsync(params object?[]? keyValues);

    Task<EntityEntry<T>> AddAsync(T entity);

    EntityEntry<T> Update(T entity);

    EntityEntry<T> Delete(T entity);

    // Maybe remove
    Task<IEnumerable<T>> GetAllAsync();

    //DEPRECATED
    IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression);
}
