using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/courses")]
[ApiController]
[Produces("application/json")]
[ValidateInput]
public class CourseController(IServiceCoordinator serviceCoordinator) : ApiBaseController
{
    private readonly IServiceCoordinator _services = serviceCoordinator;

    //GET: All courses
    /* [SkipValidation] */
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(
        [FromQuery] SearchFilterDTO searchFilterDTO
    )
    {
        var courses =
            Request.Query.Count != 0
                ? await _services.Course.GetCoursesAsync(searchFilterDTO)
                : await _services.Course.GetCoursesAsync();

        return Ok(courses);
    }

    //GET: Course by ID
    /* [SkipValidation] */
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto?>> GetCourseById(int id)
    {
        return await _services.Course.GetCourseByIdAsync(id);
    }

    // TODO: Implement update for course
    [HttpPut("{id}")]
    public Task<ActionResult<CourseDto>> UpdateCourse(int id, [FromBody] CourseDto course)
    {
        throw new NotImplementedException();

        /* var updatedCourse = await _services.Course.UpdateCourse(course);
        return Ok(updatedCourse); */
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> DeleteCourse(int id)
    {
        throw new NotImplementedException();
    }

    /* [SkipValidation] */
    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CourseCreateDto course)
    {
        var createdCourse = await _services.Course.CreateCourse(course);
        return Ok(createdCourse);
    }

    [HttpGet("{id}/students")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetCourseStudents(int id)
    {
        var users = await _services.Course.GetCourseStudentsAsync(id);

        return Ok(users);
    }

    //GET: Modules by Course ID
    [HttpGet("{id}/modules")]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetCourseModules(
        int id,
        [FromQuery] SearchFilterDTO searchFilterDTO
    )
    {
        var modules = await _services.Course.GetCourseModulesAsync(id, searchFilterDTO);

        if (modules == null)
        {
            return NotFound(
                "No modules found. Either course ID was bad or course contains no modules."
            );
        }

        return Ok(modules);
    }

    /* DEPRECATED
     **************/

    /* [SkipValidation] */
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCourse(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<CourseDto> coursePatchDocument
    )
    {
        var courseToPatchWith = await _services.Course.GetCourseByIdAsync(id);

        if (
            !TryValidateAndApplyPatch(
                coursePatchDocument,
                courseToPatchWith,
                out IActionResult errorResponse
            )
        )
        {
            return errorResponse;
        }

        await _services.Course.PatchCourse(courseToPatchWith);
        return Ok(NoContent());
    }
}
