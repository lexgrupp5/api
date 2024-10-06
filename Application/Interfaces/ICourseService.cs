using Application.DTOs;

using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICourseService : IServiceBase<Course, CourseDto>
{
    Task<CourseDto?> FindAsync(int id);
    Task<ICollection<UserDto>?> GetStudentsByIdAsync(int id, QueryParams queryParams);
    Task<ICollection<ModuleDto>?> GetModulesByIdAsync(int id, QueryParams queryParams);
    Task<(IEnumerable<CourseDto>? Courses, int TotalItemCount)> GetAllAsync(
        QueryParams? queryParams = null,
        string? searchString = null,
        DateParams? dateParams = null
    );

    /* DEPRECATED
     ***************************************************************************/
    Task<CourseDto> CreateCourse(CourseCreateDto course);
    Task PatchCourse(CourseDto courseDto);
}
