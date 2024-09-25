using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/User")]
[ApiController]
[Produces("application/json")]
//[Authorize(Roles = "Teacher")]
public class UserController : ControllerBase
{
    private readonly IServiceCoordinator _serviceCoordinator;

    public UserController(IServiceCoordinator serviceCoordinator)
    {
        _serviceCoordinator = serviceCoordinator;
    }

    //GET: Course participants by Course ID
    [HttpGet("course/{id}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersOfCourse(int id)
    {
        var users = await _serviceCoordinator.UserService.GetUsersOfCourseByIdAsync(id);
        if (users == null)
        {
            return NotFound($"Users of course with ID {id} were not found in the database.");
        }

        return Ok(users);
    }

    //GET: Course from UserName
    [HttpGet("{username}")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourseOfUser(string username)
    {
        var user = await _serviceCoordinator.UserService.GetUserByUsername(username);
        if (user == null)
        {
            return BadRequest(
                $"A user with the username {username} could not be found in the database."
            );
        }
        var usersCourseId = user.CourseId;
        if (usersCourseId == null)
        {
            return BadRequest($"{username} is not assigned to a course");
        }
        var courseId = usersCourseId.GetValueOrDefault();
        var course = await _serviceCoordinator.Course.GetCourseDtoByIdAsync(courseId);
        return Ok(course);
    }

    //PATCH: Existing User by User ID
    [HttpPatch("{username}")]
    public async Task<ActionResult> PatchUserByUsername(
        string username,
        [FromBody] JsonPatchDocument<UserForUpdateDto> patchDocument
    )
    {
        var result = await _serviceCoordinator.UserService.PatchUser(username, patchDocument);
        if (result == null)
        {
            BadRequest("User failed to get updated");
        }
        return NoContent();
    }

    [HttpGet(Name = "GetAllStudents")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllStudents()
    {
        throw new NotImplementedException();
        /* var users = await _serviceCoordinator.Identity.GetStudentsAsync();
        if (users == null)
        {
            return NotFound("No students found in the database.");
        }

        return Ok(users); */
    }

    //POST: Create new user
    [HttpPost]
    public async Task<ActionResult<UserDto?>> CreateNewUserAsync(UserForCreationDto newUser)
    {
        var userToBeCreated = await _serviceCoordinator.UserService.CreateNewUserAsync(
            newUser,
            _serviceCoordinator.User,
            _serviceCoordinator.Identity
        );
        if (userToBeCreated == null)
        {
            return BadRequest("The return body of the function call is 'null'");
        }
        return Ok(userToBeCreated);
    }
}
