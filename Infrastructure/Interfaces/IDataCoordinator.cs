using Infrastructure.Interfaces;

namespace Data;

public interface IDataCoordinator
{
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }
    
    IUserRepository Users { get; }

    Task CompleteAsync();
}
