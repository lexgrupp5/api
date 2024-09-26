using System.Linq.Expressions;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    protected AppDbContext _db;
    protected DbSet<T> DbSet { get; }

    protected RepositoryBase(AppDbContext context)
    {
        _db = context;
        DbSet = context.Set<T>();
    }

    public IQueryable<T> GetAll() => DbSet;

    public IQueryable<T> GetByConditionAsync(Expression<Func<T, bool>> expression) => DbSet.Where(expression);

    public async Task CreateAsync(T entity) => await DbSet.AddAsync(entity);

    public void UpdateAsync(T entity) => DbSet.Update(entity);

    public void DeleteAsync(T entity) => DbSet.Remove(entity);
}