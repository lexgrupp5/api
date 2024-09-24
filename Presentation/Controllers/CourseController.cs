using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Coordinator;
using Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;


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
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
        [FromQuery] SearchFilterDTO searchFilterDTO)
    {
        var courses = Request.Query.Count != 0
            ? await _serviceCoordinator.Course.GetCoursesAsync(searchFilterDTO)
            : await _serviceCoordinator.Course.GetCoursesAsync();
        
        return Ok(courses);
    }

    //GET: Course by ID
    [HttpGet("{id}", Name = "GetCourse")]
    public async Task<ActionResult<CourseDto?>> GetCourseDtoById(int id)
    {
        var dto = await _serviceCoordinator.Course.GetCourseDtoByIdAsync(id);
        if (dto == null)
        {
            return NotFound($"Course with the ID {id} was not found in the database.");
        }

        return Ok(dto);
    }

    [HttpPost(Name = "CreateCourse")]
    public async Task<ActionResult<CourseCreateDto>> CreateCourse(
        [FromBody] CourseCreateDto course)
    {    
        var createdCourse = await _serviceCoordinator.Course.CreateCourse(course);
        return CreatedAtRoute(nameof(CreateCourse), createdCourse);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<string>> PatchTodo(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<CourseDto> coursePatchDocument
    )
    {
        var courseToPatchWith = await _serviceCoordinator.Course.GetCourseDtoByIdAsync(id);
        if (courseToPatchWith == null) { return NotFound(); }

        coursePatchDocument.ApplyTo(courseToPatchWith, ModelState);
 
        if (!ModelState.IsValid || !TryValidateModel(courseToPatchWith))
        {
            var errors = ModelState.Values
                .SelectMany(modelStateEntry => modelStateEntry.Errors)
                .Select(modelError => modelError.ErrorMessage)
                .ToList();

            return BadRequest(new { Message = "Invalid state", Errors = errors });
        }

        var isPacthed = await _serviceCoordinator.Course.PatchCourse(id, courseToPatchWith);
        if (!isPacthed)
        {
            return BadRequest();
        }

        return Ok(NoContent());
    }
}