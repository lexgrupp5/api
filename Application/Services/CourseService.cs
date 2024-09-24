using Application.Interfaces;
using Data;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Validations;

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
            var invalidDateCombination = DomainDateValidation.IsValidDateCombination(
                searchFilterDTO.StartDate,
                searchFilterDTO.EndDate);

            return invalidDateCombination 
                ? ([])
                : await _dataCoordinator.Courses.GetCoursesAsync(searchFilterDTO);
        }

        //GET single course (id)
        public async Task<CourseDto?> GetCourseDtoByIdAsync(int id)
        {
            return await _dataCoordinator.Courses.GetCourseByIdAsync(id);
        }

        public async Task<CourseCreateDto> CreateCourse(CourseCreateDto course)
        {
            var courseEntity = _mapper.Map<Course>(course);
            await _dataCoordinator.Courses.CreateAsync(courseEntity);
            await _dataCoordinator.CompleteAsync();
            return _mapper.Map<CourseCreateDto>(courseEntity);
        }

        //PATCH existing course
    }
}
