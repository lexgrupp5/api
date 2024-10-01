using System.Linq.Expressions;
using Application.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.DTOs;
using Domain.Entities;
using Domain.Validations;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Application.Services
{
    public class CourseService : ServiceBase<Course>, ICourseService
    {
        //UoW
        private readonly IDataCoordinator _data;

        //Mapper
        private readonly IMapper _mapper;

        // TODO: Remove when done testing
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public CourseService(
            IDataCoordinator dataCoordinator,
            IMapper mapper,
            UserManager<User> userManager,
            AppDbContext context
        )
        {
            _data = dataCoordinator;
            _mapper = mapper;

            // TODO: Remove when done testing
            _userManager = userManager;
            _context = context;
        }

        //GET all courses
        public async Task<IEnumerable<CourseDto?>> GetCoursesAsync()
        {
            return await _data.Courses.GetCoursesAsync();
        }

        public async Task<IEnumerable<CourseDto?>> GetCoursesAsync(SearchFilterDTO searchFilterDTO)
        {
            var isValidDateCombination = EndDateValidationAttribute.IsValidDateCombination(
                searchFilterDTO.StartDate,
                searchFilterDTO.EndDate
            );

            return isValidDateCombination
                ? await _data.Courses.GetCoursesAsync(searchFilterDTO)
                : ([]);
        }

        //GET single course (id)
        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var course = await _data.Courses.GetCourseByIdAsync(id);
            if (course == null)
            {
                NotFound($"Course with the ID {id} was not found in the database.");
            }
            var courseDto = _mapper.Map<CourseDto>(course);
            return courseDto;
        }

        public async Task<CourseDto> CreateCourse(CourseCreateDto course)
        {
            var courseEntity = _mapper.Map<Course>(course);
            await _data.Courses.CreateAsync(courseEntity);
            await _data.CompleteAsync();
            return _mapper.Map<CourseDto>(courseEntity);
        }

        public async Task<IEnumerable<UserDto>> GetCourseStudentsAsync(
            int courseId,
            List<SortParams>? sorting = null,
            PageParams? paging = null
        )
        {
            List<SortParams> testSorting = [new() { Field = "Name", Descending = false }];
            PageParams testPaging = new() { Page = 1, Size = 500 };
            return await _mapper
                .ProjectTo<UserDto>(
                    _data.Users.GetQueryUsersInRole(
                        UserRoles.Student,
                        [u => u.CourseId == courseId],
                        testSorting,
                        testPaging
                    )
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<ModuleDto?>> GetCourseModulesAsync(
            int id,
            SearchFilterDTO searchFilterDto
        )
        {
            var modules = await _data.Courses.GetModulesOfCourseAsync(id, searchFilterDto);
            var moduleDtos = _mapper.Map<IEnumerable<ModuleDto>>(modules);
            return moduleDtos;
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
    }
}
