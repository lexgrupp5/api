using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
{
    public ActivityRepository(AppDbContext context)
        : base(context) { }

    // ActivityType

    public IQueryable<ActivityType> QueryActivityTypeById(int id) =>
        _db.ActivityType.Where(a => a.Id == id).AsQueryable();

    public IQueryable<ActivityType> QueryActivityTypeByName(string name) =>
        _db.ActivityType.Where(a => a.Name == name).AsQueryable();
}
