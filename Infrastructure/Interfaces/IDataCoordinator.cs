namespace Infrastructure.Interfaces;

public interface IDataCoordinator
{
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }

    IModuleRepository Modules { get; }

    Task CompleteAsync();
}