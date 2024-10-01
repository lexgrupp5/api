using Domain.DTOs;
using Infrastructure.Models;

namespace Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto?>> GetCoursesAsync();
    Task<CourseDto> GetCourseByIdAsync(int id);
    Task<IEnumerable<CourseDto?>> GetCoursesAsync(SearchFilterDTO searchFilterDTO);
    Task<CourseDto> CreateCourse(CourseCreateDto course);
    Task PatchCourse(CourseDto courseDto);
    Task<IEnumerable<UserDto>> GetCourseStudentsAsync(
        int courseId,
        List<SortParams>? sorting = null,
        PageParams? paging = null
    );
    Task<IEnumerable<ModuleDto?>> GetCourseModulesAsync(
        int id,
        SearchFilterDTO searchFilterDto
    );
}
