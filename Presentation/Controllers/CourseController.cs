using Application.DTOs;
using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/courses")]
[ApiController]
[ValidateInput]
[Produces("application/json")]
public class CourseController : ApiBaseController
{
    private readonly IServiceCoordinator _services;

    public CourseController(IServiceCoordinator services)
    {
        _services = services;
    }

    /*
     * GET: All courses
     *******************/
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(
        [FromQuery] string? search,
        [FromQuery] DateParams? dateParams,
        [FromQuery] QueryParams? queryParams
    )
    {
        var courses = await _services.Course.GetAllAsync(queryParams, search, dateParams);
        return courses != null ? Ok(courses) : NotFound();
    }

    /*
     * GET: Course by ID
     *******************/
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(int id)
    {
        var course = await _services.Course.FindAsync(id);
        return course != null ? Ok(course) : NotFound();
    }

    /*
     * GET: Students by Course ID
     *****************************/
    [HttpGet("{id}/students")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetCourseStudents(
        int id,
        [FromQuery] QueryParams queryParams
    )
    {
        var users = await _services.Course.GetStudentsByIdAsync(id, queryParams);
        return users != null ? Ok(users) : NotFound();
    }

    /*
     * GET: Modules by Course ID
     ****************************/
    [HttpGet("{id}/modules")]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetCourseModules(
        int id,
        [FromQuery] QueryParams queryParams
    )
    {
        var modules = await _services.Course.GetModulesByIdAsync(id, queryParams);
        return modules != null ? Ok(modules) : NotFound();
    }

    /*
     * POST: Create a new course
     ****************************/
    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CourseCreateDto course)
    {
        var createdCourse = await _services.Course.CreateCourse(course);
        return Ok(createdCourse);
    }

    /*
     * PUT: Course by ID
     *******************/
    [Authorize(Roles = "Teacher")]
    [HttpPut("{id}")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(
        int id,
        [FromBody] CourseUpdateDto course
    )
    {
        var result = await _services.Course.UpdateAsync(id, course);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     * DELETE: Course by ID
     ***********************/
    [Authorize(Roles = "Teacher")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse(int id, [FromBody] CourseDto course)
    {
        var result = await _services.Course.DeleteAsync(id, course);
        return result ? NoContent() : NotFound();
    }

    /* DEPRECATED
     **********************************************************************/

    /* [SkipValidation] */
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCourse(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<CourseDto> patchDocument
    )
    {
        var courseDto = await _services.Course.FindAsync(id);
        if (courseDto == null)
            return NotFound();

        if (
            !TryValidateAndApplyPatch(
                patchDocument,
                courseDto,
                out IActionResult errorResponse
            )
        )
        {
            return errorResponse;
        }

        await _services.Course.PatchCourse(courseDto);
        return Ok(NoContent());
    }
}
