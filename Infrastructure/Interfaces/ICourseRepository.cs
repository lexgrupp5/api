using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<IEnumerable<CourseDto>> GetCoursesAsync();
    Task<bool> CheckCourseExistsAsync(Course course);
}