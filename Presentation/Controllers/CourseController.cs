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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _serviceCoordinator.CourseService.GetCoursesAsync();
            return Ok(courses);
        }
    }
}
