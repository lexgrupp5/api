namespace Infrastructure.Interfaces;

public interface IDataCoordinator
{
    IActivityRepository Activities { get; }
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }
    IUserRepository Users { get; }

    Task<int> CompleteAsync();

    bool IsTracked<TEntity>(TEntity entity)
        where TEntity : class;
}
