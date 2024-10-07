using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICourseRepository : IRepositoryBase<Course>
{

    /* DEPRECATED */

    /*
    Task<ICollection<CourseDto>?> GetCoursesAsync();
    Task<Course?> GetCourseByIdAsync(int id);
    Task<bool> CheckCourseExistsAsync(Course course);
    Task<ICollection<CourseDto>?> GetCoursesAsync(SearchFilterDTO searchFilterDTO);
    Task<ICollection<Module>?> GetModulesOfCourseAsync(int id, SearchFilterDTO searchFilterDto);
     */
}
