using Infrastucture.Interfaces;

namespace Data;

public interface IDataCoordinator
{
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }

    Task CompleteAsync();
}
