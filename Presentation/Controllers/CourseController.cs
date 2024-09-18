using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

using Service;

namespace Presentation.Controllers
{
    [Route("api/courses")]
    [ApiController]
    [Produces("application/json")]

    public class CourseController : ControllerBase
    {
        private readonly IServiceCoordinator _serviceCoordinator;

        public CourseController(IServiceCoordinator serviceCoordinator)
        {
            _serviceCoordinator = serviceCoordinator;
        }

        //GET: All courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _serviceCoordinator.CourseService.GetCoursesAsync();
            return Ok(courses);
        }

        //GET: Course by ID
        [HttpGet("{id}", Name = "GetCourse")]
        public async Task<ActionResult<CourseDto?>> GetCourseDtoById(int id)
        {
            var dto = await _serviceCoordinator.CourseService.GetCourseDtoByIdAsync(id);
            if (dto == null)
            {
                return NotFound($"Course with the ID {id} was not found in the database.");
            }

            return Ok(dto);
        }
    }
}
