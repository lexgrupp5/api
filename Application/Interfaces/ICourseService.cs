using Application.DTOs;
using Domain.DTOs;
using Infrastructure.Models;

namespace Application.Interfaces;

public interface ICourseService
{
    Task<CourseDto?> FindAsync(int id);
    Task<ICollection<UserDto>?> GetStudentsByIdAsync(int id, QueryParams queryParams);
    Task<ICollection<ModuleDto>?> GetModulesByIdAsync(int id, QueryParams queryParams);
    Task<IEnumerable<CourseDto>?> GetAllAsync(
        QueryParams? queryParams = null,
        string? searchString = null,
        DateParams? dateParams = null
    );

    /* DEPRECATED
     ***************************************************************************/
    Task<CourseDto> CreateCourse(CourseCreateDto course);
    Task PatchCourse(CourseDto courseDto);
}
