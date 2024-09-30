using Application.Interfaces;

using AutoMapper;

using Domain.DTOs;
using Domain.Entities;
using Domain.Validations;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Interfaces;

namespace Application.Services
{
    public class CourseService : ServiceBase<Course>, ICourseService
    {
        //UoW
        private readonly IDataCoordinator _dataCoordinator;
        //Mapper
        private readonly IMapper _mapper;

        public CourseService(IDataCoordinator dataCoordinator, IMapper mapper)
        {
            _dataCoordinator = dataCoordinator;
            _mapper = mapper;
        }

        //GET all courses
        public async Task<IEnumerable<CourseDto?>> GetCoursesAsync()
        {
            return await _dataCoordinator.Courses.GetCoursesAsync();
        }

        public async Task<IEnumerable<CourseDto?>> GetCoursesAsync(
            SearchFilterDTO searchFilterDTO)
        {
            var isValidDateCombination = EndDateValidationAttribute.IsValidDateCombination(
                searchFilterDTO.StartDate,
                searchFilterDTO.EndDate);

            return isValidDateCombination
                ? await _dataCoordinator.Courses.GetCoursesAsync(searchFilterDTO)
                : ([]);
        }

        //GET single course (id)
        public async Task<CourseDto> GetCourseDtoByIdAsync(int id)
        {
            var course = await _dataCoordinator.Courses.GetCourseByIdAsync(id);
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
            await _dataCoordinator.Courses.CreateAsync(courseEntity);
            await _dataCoordinator.CompleteAsync();
            return _mapper.Map<CourseDto>(courseEntity);
        }

        public async Task PatchCourse(CourseDto courseDto)
        {
            var course = await _dataCoordinator.Courses
                .GetByConditionAsync(course =>course.Id == courseDto.Id)
                .FirstOrDefaultAsync();

            if (course == null) { NotFound($"Course with the ID {courseDto.Id} was not found in the database."); }
            
            _mapper.Map(courseDto, course);
            
            await _dataCoordinator.CompleteAsync();
        }
    }
}
