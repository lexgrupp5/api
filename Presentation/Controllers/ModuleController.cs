using Service;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/modules")]
[ApiController]
[Produces("application/json")]
public class ModuleController : ControllerBase
{
    private readonly IServiceCoordinator _serviceCoordinator;

    public ModuleController(IServiceCoordinator serviceCoordinator)
    {
        _serviceCoordinator = serviceCoordinator;
    }

    //GET: Modules by Course ID
    [HttpGet("course/{id}")]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModulesOfCourse(int id)
    {
        var modules = await _serviceCoordinator.ModuleService.GetModulesOfCourseIdAsync(id);
        if (modules == null)
        {
            return NotFound("No modules found. Either course ID was bad or course contains no modules.");
        }

        return Ok(modules);
    }

}

