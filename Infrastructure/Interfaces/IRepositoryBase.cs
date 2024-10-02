using System.Linq.Expressions;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Interfaces;

public interface IRepositoryBase<T>
    where T : class
{
    IQueryable<T> GetQuery(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    );
    IQueryable<T> GetQueryById(params object?[]? keyValues);
    Task<bool> ExistsAsync(params object?[]? keyValues);
    EntityEntry<T> Update(T entity);
    EntityEntry<T> Delete(T entity);
    Task<EntityEntry<T>> CreateAsync(T entity);

    // Maybe remove
    IEnumerable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();

    //DEPRECATED
    IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression);
}
