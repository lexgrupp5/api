namespace Infrastructure.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);
    Task CreateAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
}