using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Coordinators;

public class DataCoordinator : IDataCoordinator, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _db;
    private readonly Lazy<IActivityRepository> _activityRepository;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<IUserRepository> _userRepository;

    public IActivityRepository Activities => _activityRepository.Value;
    public ICourseRepository Courses => _courseRepository.Value;
    public IModuleRepository Modules => _moduleRepository.Value;
    public IUserRepository Users => _userRepository.Value;

    public DataCoordinator(
        AppDbContext context,
        Lazy<IActivityRepository> activityRepository,
        Lazy<ICourseRepository> courseRepository,
        Lazy<IModuleRepository> moduleRepository,
        Lazy<IUserRepository> userRepository
    )
    {
        _db = context;
        _activityRepository = activityRepository;
        _courseRepository = courseRepository;
        _moduleRepository = moduleRepository;
        _userRepository = userRepository;
    }

    public DbSet<T> Set<T>()
        where T : class => _db.Set<T>();

    public DbContext Context => _db;

    public async Task<int> CompleteAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public bool IsTracked<T>(T entity)
        where T : class => _db.Entry(entity).State != EntityState.Detached;

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
