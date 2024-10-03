using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/modules")]
[ApiController]
[ValidateInput]
[Produces("application/json")]
public class ModuleController : ApiBaseController
{
    private readonly IServiceCoordinator _services;

    public ModuleController(IServiceCoordinator services)
    {
        _services = services;
    }

    /*
     *
     ****/
    [HttpGet("{id}")]
    public async Task<ActionResult<ModuleDto>> GetModuleById(int id)
    {
        var result = await _services.Module.FindAsync(id);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     *
     ****/
    [HttpGet("{id}/activities")]
    public async Task<ActionResult<ActivityDto[]>> GetActivitiesByModuleId(int id)
    {
        var result = await _services.Module.GetActivitiesByModuleIdAsync(id);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     *
     ****/
    [HttpPost]
    public async Task<ActionResult<ModuleDto>> CreateModule([FromBody] ModuleCreateDto dto)
    {
        var result = await _services.Module.CreateAsync(dto);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     *
     ****/
    [HttpPut("{id}")]
    public async Task<ActionResult<ModuleDto>> UpdateModule(
        [FromRoute] int id,
        [FromBody] ModuleUpdateDto module
    )
    {
        var result = await _services.Module.UpdateAsync(id, module);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     *
     ****/
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteModule(int id, [FromBody] ModuleDto module)
    {
        var result = await _services.Module.DeleteAsync(id, module);
        return result ? NoContent() : BadRequest();
    }

    /* DEPRECATED
     *************************************************************/


    /*
     *
     ****/
    [HttpPatch("{id}")]
    public async Task<ActionResult<ModuleDto>> PatchModule(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<ModuleDto> patchDocument
    )
    {
        var currentDto = await _services.Module.FindAsync(id);
        if (currentDto == null)
            return NotFound();

        if (!TryValidateAndApplyPatch(patchDocument, currentDto, out IActionResult errorResponse))
        {
            return BadRequest(errorResponse);
        }

        return Ok(await _services.Module.PatchModule(currentDto));
    }

    /* [HttpPost]
   public async Task<ActionResult<ModuleForCreationDto>> CreateModule(
       ModuleCreateModel moduleToCreate
   )
   {
       var result = await _services.Module.CreateModuleAsync(moduleToCreate);
       return Ok(result);
   } */

    //POST: Create activity
    /* [SkipValidation] */
    /* [HttpPost("createActivity")]
    public async Task<ActionResult<ActivityForCreationDto>> CreateActivity(
        ActivityCreateDto activityToCreate
    )
    {
        var result = await _services.Module.CreateActivityAsync(activityToCreate);
        return Ok(result);
    } */

    //PATCH: Patch a module
    /* [SkipValidation] */
    /*  */

    //PATCH: Patch an activity
    /* [SkipValidation] */
    /*  */
}
