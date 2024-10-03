using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/users")]
[ApiController]
[ValidateInput]
[Produces("application/json")]
public class UserController : ApiBaseController
{
    private readonly IServiceCoordinator _services;

    public UserController(IServiceCoordinator services)
    {
        _services = services;
    }

    /*
     *
     ****/
    [Authorize(Roles = "Teacher")]
    [HttpGet(Name = "GetAllStudents")]
    public Task<ActionResult<IEnumerable<UserDto>?>> GetAllStudents()
    {
        throw new NotImplementedException();
    }

    /*
     *
     ****/
    // TODO: implement
    private Task<ActionResult<UserDto>> CreateNewUserAsync(UserCreateDto dto)
    {
        throw new NotImplementedException();
    }

    /*
     *
     ****/
    [HttpPut("{id}")]
    public Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UserForUpdateDto user)
    {
        throw new NotImplementedException();
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
        var result = await _services.User.PatchUser(username, patchDocument);
        if (result == null)
        {
            BadRequest("User failed to get updated");
        }
        return NoContent();
    }

    /*
     * POST: Create new user
     ****/
    [HttpPost]
    public async Task<ActionResult<UserDto?>> CreateNewUserAsync(UserForCreationDto newUser)
    {
        var userToBeCreated = await _services.User.CreateNewUserAsync(
            newUser,
            _services.UserManager,
            _services.Identity
        );
        if (userToBeCreated == null)
        {
            return BadRequest("The return body of the function call is 'null'");
        }
        return Ok(userToBeCreated);
    }
}
