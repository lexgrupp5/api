using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/User")]
[ApiController]
[Produces("application/json")]
//[Authorize(Roles = "Teacher")]
[ValidateInput]
public class UserController : ControllerBase
{
    private readonly IServiceCoordinator _services;

    public UserController(IServiceCoordinator serviceCoordinator)
    {
        _services = serviceCoordinator;
    }

    /*
     *
     ****/
    /* [SkipValidation] */
    [Authorize(Roles = "Teacher")]
    [HttpGet(Name = "GetAllStudents")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllStudents()
    {
        var users = await _services.UserService.GetAllUsersAsync();
        return users == null ? NotFound("No users found in the database.") : Ok(users);
    }

    /*
     * POST: Create new user
     ****/
    /* [SkipValidation] */
    [HttpPost]
    public async Task<ActionResult<UserDto?>> CreateNewUserAsync(UserForCreationDto newUser)
    {
        var userToBeCreated = await _services.UserService.CreateNewUserAsync(
            newUser,
            _services.User,
            _services.Identity
        );
        if (userToBeCreated == null)
        {
            return BadRequest("The return body of the function call is 'null'");
        }
        return Ok(userToBeCreated);
    }

    // TODO: Implement update for user
    [HttpPut("{id}")]
    public Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UserForUpdateDto user)
    {
        throw new NotImplementedException();

        /* var updatedUser = await _services.UserService.UpdateUser(user);
        return Ok(updatedUser); */
    }

    /*
     * DEPRECATED
     ************/

    //PATCH: Existing User by User ID
    /* [SkipValidation] */
    [HttpPatch("{username}")]
    public async Task<ActionResult> PatchUserByUsername(
        string username,
        [FromBody] JsonPatchDocument<UserForUpdateDto> patchDocument
    )
    {
        var result = await _services.UserService.PatchUser(username, patchDocument);
        if (result == null)
        {
            BadRequest("User failed to get updated");
        }
        return NoContent();
    }

    //GET: Course from UserName
    /* [SkipValidation] */
    [HttpGet("{username}")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourseOfUser(string username)
    {
        var user = await _services.UserService.GetUserByUsername(username);
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
        var course = await _services.Course.FindAsync(courseId);
        return Ok(course);
    }

    //GET: Course participants by Course ID
    /* [SkipValidation] */
    [HttpGet("course/{id}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersOfCourse(int id)
    {
        var users = await _services.UserService.GetUsersOfCourseByIdAsync(id);
        if (users == null)
        {
            return NotFound($"Users of course with ID {id} were not found in the database.");
        }

        return Ok(users);
    }
}
