using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<bool> CheckCourseExistsAsync(Course course);
}