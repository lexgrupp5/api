namespace Infrastructure.Interfaces;

public interface IDataCoordinator
{
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }
    IUserRepository Users { get; }

    Task<int> CompleteAsync();

    bool IsEntityTracked<TEntity>(TEntity entity)
        where TEntity : class;
}
