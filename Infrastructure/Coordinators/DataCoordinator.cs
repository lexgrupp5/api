using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Coordinators;

public class DataCoordinator(
    AppDbContext context,
    Lazy<ICourseRepository> courseRepository,
    Lazy<IModuleRepository> moduleRepository,
    Lazy<IActivityRepository> activityRepository,
    Lazy<IUserRepository> userRepository
) : IDataCoordinator, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _db = context;
    private readonly Lazy<ICourseRepository> _courseRepository = courseRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository = moduleRepository;
    private readonly Lazy<IActivityRepository> _activityRepository = activityRepository;
    private readonly Lazy<IUserRepository> _userRepository = userRepository;

    public ICourseRepository Courses => _courseRepository.Value;
    public IModuleRepository Modules => _moduleRepository.Value;
    public IActivityRepository Activities => _activityRepository.Value;
    public IUserRepository Users => _userRepository.Value;

    public async Task<int> CompleteAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public bool IsEntityTracked<TEntity>(TEntity entity)
        where TEntity : class => _db.Entry(entity).State != EntityState.Detached;

    public void Dispose()
    {
        _db.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_db is not null)
            await _db.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
