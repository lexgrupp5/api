using Infrastructure.Interfaces;
using Infrastructure.Persistence.Repositories;

namespace Data;

public class DataCoordinator : IDataCoordinator, IDisposable
{
    private readonly AppDbContext _appDbContext;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<IActivityRepository> _activityRepository;

    public ICourseRepository Courses => _courseRepository.Value;
    public IModuleRepository Modules => _moduleRepository.Value;
    public IActivityRepository Activities => _activityRepository.Value;

    public DataCoordinator(AppDbContext appDbContext, Lazy<ICourseRepository> courseRepository, Lazy<IModuleRepository> moduleRepository, Lazy<IActivityRepository> activityRepository)
    {
        _appDbContext = appDbContext;
        _courseRepository = courseRepository;
        _moduleRepository = moduleRepository;
        _activityRepository = activityRepository;
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
    }

    public async Task CompleteAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }
}