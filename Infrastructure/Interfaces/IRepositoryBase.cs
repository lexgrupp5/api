using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    public IQueryable<T> GetAll();
    IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
    Task CreateAsync(T entity);
}