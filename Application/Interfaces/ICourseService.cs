using Domain.DTOs;

namespace Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto?>> GetCoursesAsync();
    Task<CourseDto?> GetCourseDtoByIdAsync(int id);
}
