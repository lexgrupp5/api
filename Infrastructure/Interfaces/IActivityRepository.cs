using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IActivityRepository : IRepositoryBase<Activity>
{
    IQueryable<ActivityType> QueryActivityTypeByName(string name);
    IQueryable<ActivityType> QueryActivityTypeById(int id);
}
