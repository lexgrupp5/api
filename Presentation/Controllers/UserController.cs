using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Presentation.Controllers;

[Route("api/User")]
[ApiController]
[Produces("application/json")]
//[Authorize(Roles = "Teacher")]
public class UserController: ControllerBase
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
    
    //PATCH: Existing User by User ID
    [HttpPatch("{username}")]
    public async Task<ActionResult> PatchUserById(string username, [FromBody]JsonPatchDocument<UserForUpdateDto> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest("Patch document is null");
        }
        var userToBeUpdated = await _serviceCoordinator.UserService.GetUserByUsername(username);
        if (userToBeUpdated == null) { BadRequest( $"A user with the username {username} could not be found in the database."); }

        await _serviceCoordinator.UserService.PatchUser(userToBeUpdated!, patchDocument);
        return NoContent();
    }

    [HttpGet(Name = "GetAllStudents")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllStudents()
    {
        var users = await _serviceCoordinator.Identity.GetStudentsAsync();
        if (users == null)
        {
            return NotFound("No students found in the database.");
        }

        return Ok(users);
    }

    //POST: Create new user
    [HttpPost]
    public async Task<ActionResult<UserDto?>> CreateNewUserAsync(UserForCreationDto newUser)
    {
        var userToBeCreated = await _serviceCoordinator.UserService.CreateNewUserAsync(newUser, _serviceCoordinator.User, _serviceCoordinator.Identity);
        if (userToBeCreated == null)
        {
            return BadRequest("The return body of the function call is 'null'");
        }
        return Ok(userToBeCreated);
    }
}