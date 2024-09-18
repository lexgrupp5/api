using Application.Interfaces;

namespace Service;

public class ServiceCoordinator : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService;
    
    public ICourseService CourseService => _courseService.Value;

    public ServiceCoordinator(Lazy<ICourseService> courseService)
    {
        _courseService = courseService;
    }
}