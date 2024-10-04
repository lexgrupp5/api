using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Services;

using Microsoft.EntityFrameworkCore;

using Shared.Extensions;

namespace Application.Services;

public class CourseService : ServiceBase<Course, CourseDto>, ICourseService
{
    public CourseService(IDataCoordinator dataCoordinator, IMapper mapper)
        : base(dataCoordinator, mapper) { }

    /*
     *
     ****/
    public async Task<IEnumerable<CourseDto>?> GetAllAsync(
        QueryParams? queryParams = null,
        string? searchString = null,
        DateParams? dateParams = null
    )
    {
        var filters = new List<Expression<Func<Course, bool>>>();

        var (sorting, paging) = ParseQueryParams(queryParams);

        if (searchString != null)
            filters.AddNotNull(CreateSearchFilter(searchString, c => c.Name, c => c.Description));

        if (dateParams != null)
            filters.AddNotNull(CreateDateRangeFilter(dateParams.StartDate, dateParams.EndDate));

        return await _mapper
            .ProjectTo<CourseDto>(_data.Courses.GetQuery(filters, sorting, paging))
            .ToListAsync();
    }

    /*
     *
     ****/
    public async Task<CourseDto?> FindAsync(int id)
    {
        var query = _data.Courses.GetQueryById(id);
        return await _mapper.ProjectTo<CourseDto>(query).FirstOrDefaultAsync();
    }

    /*
     *
     ****/
    public async Task<ICollection<UserDto>?> GetStudentsByIdAsync(int id, QueryParams queryParams)
    {
        var (sorting, paging) = ParseQueryParams(queryParams);
        var query = _data.Users.GetQueryUsersInRole(
            UserRoles.Student,
            [u => u.CourseId == id],
            sorting,
            paging
        );

        return await _mapper.ProjectTo<UserDto>(query).ToListAsync();
    }

    /*
     *
     ****/
    public async Task<ICollection<ModuleDto>?> GetModulesByIdAsync(int id, QueryParams queryParams)
    {
        var (sorting, paging) = ParseQueryParams(queryParams);
        var query = _data.Modules.GetQuery([m => m.CourseId == id], sorting, paging);

        return await _mapper.ProjectTo<ModuleDto>(query).ToListAsync();
    }

    /*
     *
     ****/
    public async Task<CourseDto?> Update(CourseUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var current = await _data.Courses.GetQueryById(dto.Id).FirstOrDefaultAsync();
        if (current == null)
            NotFound();

        _mapper.Map(dto, current);
        await _data.CompleteAsync();
        return _mapper.Map<CourseDto>(current);
    }

    /* DEPRECATED
     ***************************************************************************/

    public async Task<CourseDto> CreateCourse(CourseCreateDto course)
    {
        var courseEntity = _mapper.Map<Course>(course);
        await _data.Courses.AddAsync(courseEntity);
        await _data.CompleteAsync();
        return _mapper.Map<CourseDto>(courseEntity);
    }

    public async Task PatchCourse(CourseDto courseDto)
    {
        var course = await _data
            .Courses.GetByConditionAsync(course => course.Id == courseDto.Id)
            .FirstOrDefaultAsync();

        if (course == null)
        {
            NotFound($"Course with the ID {courseDto.Id} was not found in the database.");
        }

        _mapper.Map(courseDto, course);

        await _data.CompleteAsync();
    }

    /* Private Helpers
     ***************************************************************************/

    private static Expression<Func<Course, bool>>? CreateDateRangeFilter(
        DateTime? startDate,
        DateTime? endDate
    )
    {
        if (startDate is null || endDate is null)
            return null;

        if (!(startDate > DateTime.MinValue) || !(endDate > DateTime.MinValue))
            return null;

        return course => course.StartDate >= startDate && course.EndDate <= endDate;
    }
}
