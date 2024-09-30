using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<IEnumerable<CourseDto>> GetCoursesAsync();
    Task<Course?> GetCourseByIdAsync(int id);
    Task<bool> CheckCourseExistsAsync(Course course);
    Task<IEnumerable<CourseDto?>> GetCoursesAsync(SearchFilterDTO searchFilterDTO);
    
    Task<IEnumerable<Module?>?> GetModulesOfCourseAsync(int id, SearchFilterDTO searchFilterDto);

}