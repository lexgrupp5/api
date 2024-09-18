using Application.Interfaces;

namespace Service;

public interface IServiceCoordinator
{
    ICourseService CourseService { get; }
    
}