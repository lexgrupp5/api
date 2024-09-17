using Domain.Entities;

using Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckCourseExistsAsync(Course course)
    {
        return await _context.Courses.AnyAsync(c => c.Name == course.Name);
    }
}