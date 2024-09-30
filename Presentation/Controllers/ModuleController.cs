using Application.Interfaces;
using Application.Models;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/modules")]
[ApiController]
[Produces("application/json")]
[ValidateInput]
public class ModuleController(IServiceCoordinator serviceCoordinator) : ApiBaseController
{
    private readonly IServiceCoordinator _serviceCoordinator = serviceCoordinator;
    

    //GET: Modules by Course ID
/* [SkipValidation] */
    [HttpGet("course/{id}")]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModulesOfCourse(int id,  [FromQuery] SearchFilterDTO searchFilterDTO)
    {
        var modules = await _serviceCoordinator.Module.GetModulesOfCourseIdAsync(id, searchFilterDTO);
        
        if (modules == null)
        {
            return NotFound(
                "No modules found. Either course ID was bad or course contains no modules."
            );
        }

    //GET: Activities of a module by Module ID
    /* [SkipValidation] */
    [HttpGet("{id}/activities")]
    public async Task<ActionResult<ActivityDto[]>> GetActivitiesOfModule(int id)
    {
        /* var module = await _serviceCoordinator.Module.GetModuleByIdWithActivitiesAsync(id); */

        var module = await _serviceCoordinator.Module.GetModuleByIdAsync<ModuleDto>(id);
        if (module == null)
        {
            return NotFound($"Module with the ID {id} was not found in the database.");
        }

        var activities = module.Activities;

        return Ok(activities);
    }

    //POST: Create a module
    /* [SkipValidation] */
    [HttpPost]
    public async Task<ActionResult<ModuleForCreationDto>> CreateModule(
        ModuleCreateModel moduleToCreate
    )
    {
        var result = await _serviceCoordinator.Module.CreateModuleAsync(moduleToCreate);
        return Ok(result);
    }

    //POST: Create activity
    /* [SkipValidation] */
    [HttpPost("createActivity")]
    public async Task<ActionResult<ActivityForCreationDto>> CreateActivity(
        ActivityCreateModel activityToCreate
    )
    {
        var result = await _serviceCoordinator.Module.CreateActivityAsync(activityToCreate);
        return Ok(result);
    }
    //PATCH: Patch a module
    /* [SkipValidation] */
    [HttpPatch("module/{id}")]
    public async Task<IActionResult> PatchModule(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<ModuleToPatchDto> modulePatchDocument)
    {
        var moduleToPatch = await _serviceCoordinator.Module.GetModule(id);

        if (!TryValidateAndApplyPatch(
                modulePatchDocument,
                moduleToPatch,
                out IActionResult errorResponse))
        {
            return errorResponse;
        }

        await _serviceCoordinator.Module.PatchModule(moduleToPatch);
        return Ok(NoContent());
    }

    //PATCH: Patch an activity
    /* [SkipValidation] */
    [HttpPatch("activity/{id}")]
    public async Task<IActionResult> PatchActivity(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<ActivityDto> activityPatchDocument)
    {
        var activityToPatch = await _serviceCoordinator.Module.GetActivityByIdAsync(id);

        if (!TryValidateAndApplyPatch(
                activityPatchDocument,
                activityToPatch,
                out IActionResult errorResponse))
        {
            return errorResponse;
        }

        await _serviceCoordinator.Module.PatchActivity(activityToPatch);
        return Ok(NoContent());
    }
}