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
    private readonly IServiceCoordinator _serviceCoordinator = serviceCoordinator;

    //GET: All courses
    /* [SkipValidation] */
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
        [FromQuery] SearchFilterDTO searchFilterDTO
    )
    {
        var courses =
            Request.Query.Count != 0
                ? await _serviceCoordinator.Course.GetCoursesAsync(searchFilterDTO)
                : await _serviceCoordinator.Course.GetCoursesAsync();

        return Ok(courses);
    }

    //GET: Course by ID
    /* [SkipValidation] */
    [HttpGet("{id}", Name = "GetCourse")]
    public async Task<ActionResult<CourseDto?>> GetCourseDtoById(int id)
    {
        return await _serviceCoordinator.Course.GetCourseDtoByIdAsync(id);
    }

    /* [SkipValidation] */
    [HttpPost(Name = "CreateCourse")]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CourseCreateDto course)
    {
        var createdCourse = await _serviceCoordinator.Course.CreateCourse(course);
        return Ok(createdCourse);
    }

    /* [SkipValidation] */
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCourse(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<CourseDto> coursePatchDocument
    )
    {
        var courseToPatchWith = await _serviceCoordinator.Course.GetCourseDtoByIdAsync(id);

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

        await _serviceCoordinator.Course.PatchCourse(courseToPatchWith);
        return Ok(NoContent());
    }
    //GET: Modules by Course ID
    [HttpGet("modules/{id}")]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModulesOfCourse(int id,  [FromQuery] SearchFilterDTO searchFilterDTO)
    {
        var modules = await _serviceCoordinator.Course.GetModulesOfCourseIdAsync(id, searchFilterDTO);
        
        if (modules == null)
        {
            return NotFound(
                "No modules found. Either course ID was bad or course contains no modules."
            );
        }

        return Ok(modules);
    }
}
