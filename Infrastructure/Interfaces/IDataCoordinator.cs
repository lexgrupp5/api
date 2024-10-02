using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interfaces;

public interface IDataCoordinator
{
    IActivityRepository Activities { get; }
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }
    IUserRepository Users { get; }

    DbSet<T> Set<T>()
        where T : class;

    DbContext Context { get; }

    Task<int> CompleteAsync();

    bool IsTracked<TEntity>(TEntity entity)
        where TEntity : class;
}
