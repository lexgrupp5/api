using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Application.Interfaces;
using Data;
using AutoMapper;
using Domain.DTOs;

namespace Application.Services
{
    public class CourseService : ICourseService
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
        //GET single course (id)
        public async Task<CourseDto?> GetCourseDtoByIdAsync(int id)
        {
            return await _dataCoordinator.Courses.GetCourseByIdAsync(id);

        }

        //POST new course
        //PATCH existing course
    }
}
