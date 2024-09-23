using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
    private readonly IMapper _mapper;
    public CourseRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
    {
        var query = GetAll();

        var queryResults = await query
            .Include(c => c.Modules)
            .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return queryResults;
           
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int id)
    {
        var result = await GetByConditionAsync(m => m.Id.Equals(id)).Include(m => m.Modules).FirstOrDefaultAsync();
        if (result == null) { return null; }
        return _mapper.Map<CourseDto>(result);
    }

    public async Task<bool> CheckCourseExistsAsync(Course course)
    {
        return await _context.Courses.AnyAsync(c => c.Name == course.Name);
    }
}