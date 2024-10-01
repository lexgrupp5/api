using Application.Interfaces;
using Application.Models;
using Domain.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/modules")]
[ApiController]
[Produces("application/json")]
[ValidateInput]
public class ModuleController(IServiceCoordinator serviceCoordinator) : ApiBaseController
{
    private readonly IServiceCoordinator _services = serviceCoordinator;

    [HttpGet("{id}")]
    public Task<ActionResult<ModuleDto>> GetModuleById(int id)
    {
        throw new NotImplementedException();
    }

    //GET: Activities of a module by Module ID
    /* [SkipValidation] */
    [HttpGet("{id}/activities")]
    public async Task<ActionResult<ActivityDto[]>> GetModuleActivities(int id)
    {
        /* var module = await _serviceCoordinator.Module.GetModuleByIdWithActivitiesAsync(id); */

        var module = await _services.Module.GetModuleByIdAsync<ModuleDto>(id);
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
        var result = await _services.Module.CreateModuleAsync(moduleToCreate);
        return Ok(result);
    }

    //POST: Create activity
    /* [SkipValidation] */
    [HttpPost("createActivity")]
    public async Task<ActionResult<ActivityForCreationDto>> CreateActivity(
        ActivityCreateModel activityToCreate
    )
    {
        var result = await _services.Module.CreateActivityAsync(activityToCreate);
        return Ok(result);
    }

    // TODO: Implement update for module
    [HttpPut("{id}")]
    public Task<ActionResult<ModuleDto>> UpdateModule(int id, [FromBody] ModuleDto module)
    {
        throw new NotImplementedException();

        /* var updatedModule = await _services.Module.UpdateModule(module);
        return Ok(updatedModule); */
    }

    /* DEPRECATED
     **************/

    //PATCH: Patch a module
    /* [SkipValidation] */
    [HttpPatch("module/{id}")]
    public async Task<IActionResult> PatchModule(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<ModuleToPatchDto> modulePatchDocument
    )
    {
        var moduleToPatch = await _services.Module.GetModule(id);

        if (
            !TryValidateAndApplyPatch(
                modulePatchDocument,
                moduleToPatch,
                out IActionResult errorResponse
            )
        )
        {
            return errorResponse;
        }

        await _services.Module.PatchModule(moduleToPatch);
        return Ok(NoContent());
    }

    //PATCH: Patch an activity
    /* [SkipValidation] */
    [HttpPatch("activity/{id}")]
    public async Task<IActionResult> PatchActivity(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<ActivityDto> activityPatchDocument
    )
    {
        var activityToPatch = await _services.Module.GetActivityByIdAsync(id);

        if (
            !TryValidateAndApplyPatch(
                activityPatchDocument,
                activityToPatch,
                out IActionResult errorResponse
            )
        )
        {
            return errorResponse;
        }

        await _services.Module.PatchActivity(activityToPatch);
        return Ok(NoContent());
    }
}
