using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class ActivityRepository(AppDbContext context) : RepositoryBase<Activity>(context), IActivityRepository;