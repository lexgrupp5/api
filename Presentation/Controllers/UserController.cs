using Service;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/User")]
[ApiController]
[Produces("application/json")]
//[Authorize(Roles = "Teacher")]
public class UserController(IServiceCoordinator serviceCoordinator) : ControllerBase
{
    [HttpGet("{id}", Name = "GetModule")]
    public async Task<ActionResult<ModuleDto>> GetModule(int id)
    {
        var module = await serviceCoordinator.ModuleService.GetModuleByIdWithActivitiesAsync(id);
        if (module == null)
        {
            return NotFound($"Module with the ID {id} was not found in the database.");
        }

        return Ok(module);
    }
}