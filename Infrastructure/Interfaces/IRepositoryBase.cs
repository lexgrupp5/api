using System.Linq.Expressions;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Interfaces;

public interface IRepositoryBase<T>
    where T : class
{
    IQueryable<T> GetQuery(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        ICollection<SortParams>? sorting = null,
        PageParams? paging = null,
        IEnumerable<Expression<Func<T, object>>>? includes = null
    );
    IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression);
    EntityEntry<T> Update(T entity);
    EntityEntry<T> Delete(T entity);
    Task<EntityEntry<T>> CreateAsync(T entity);
    IEnumerable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();
}
