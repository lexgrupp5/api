using Infrastructure.Interfaces;

namespace Data;

public interface IDataCoordinator
{
    ICourseRepository Courses { get; }

    Task CompleteAsync();
}