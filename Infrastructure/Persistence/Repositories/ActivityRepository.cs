using Infrastructure.Interfaces;

using Activity = Domain.Entities.Activity;

namespace Infrastructure.Persistence.Repositories;

public class ActivityRepository(AppDbContext context) : RepositoryBase<Activity>(context), IActivityRepository;