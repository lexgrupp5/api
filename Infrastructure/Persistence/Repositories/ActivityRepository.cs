using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
{
    public ActivityRepository(AppDbContext context) : base(context)
    {
    }

    
}