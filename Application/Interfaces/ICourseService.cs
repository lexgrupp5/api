using Domain.DTOs;

using Microsoft.AspNetCore.JsonPatch;

namespace Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto?>> GetCoursesAsync();
    Task<CourseDto?> GetCourseDtoByIdAsync(int id);
    Task<IEnumerable<CourseDto?>> GetCoursesAsync(SearchFilterDTO searchFilterDTO);
    Task<CourseCreateDto> CreateCourse(CourseCreateDto course);
    Task<bool> PatchCourse(int id, CourseDto courseDto);
}
