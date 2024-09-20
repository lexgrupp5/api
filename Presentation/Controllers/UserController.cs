using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

using Microsoft.AspNetCore.Identity;

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
    
    [HttpGet(Name = "GetAllStudents")]
    public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAllStudents()
    {
        var users = await _serviceCoordinator.Identity.GetStudentsAsync();
        if (users == null)
        {
            return NotFound("No students found in the database.");
        }

        return Ok(users);
    }
}