using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

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
    [Authorize]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var courses = await _serviceCoordinator.Course.GetCoursesAsync();
        return Ok(courses);
    }

    //GET: Course by ID
    [HttpGet("{id}", Name = "GetCourse")]
    [Authorize]
    public async Task<ActionResult<CourseDto?>> GetCourseDtoById(int id)
    {
        var dto = await _serviceCoordinator.Course.GetCourseDtoByIdAsync(id);
        if (dto == null)
        {
            return NotFound($"Course with the ID {id} was not found in the database.");
        }

        return Ok(dto);
    }
}
