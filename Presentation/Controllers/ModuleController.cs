using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Models;

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

    //GET: Activities of a module by Module ID
    [HttpGet("activities/{id}")]
    public async Task<ActionResult<ActivityDto[]>> GetActivitiesOfModule(int id)
    {
        var module = await _serviceCoordinator.ModuleService.GetModuleByIdWithActivitiesAsync(id);
        if (module == null)
        {
            return NotFound($"Module with the ID {id} was not found in the database.");
        }

        var activities = module.Activities;

        return Ok(activities);
    }

    //POST: Create a module
    [HttpPost]
    public async Task<ActionResult<ModuleForCreationDto>> CreateModule(ModuleCreateModel moduleToCreate)
    {
        var result = await _serviceCoordinator.ModuleService.CreateModuleAsync(moduleToCreate);
        return Ok(result);
    }
}

