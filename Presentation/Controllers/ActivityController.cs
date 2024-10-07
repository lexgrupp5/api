using Application.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/activities")]
[ApiController]
[ValidateInput]
[Produces("application/json")]
public class ActivityController : ApiBaseController
{
    private readonly IServiceCoordinator _services;

    public ActivityController(IServiceCoordinator services)
    {
        _services = services;
    }

    /*
     *
     ****/
    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivityById(int id)
    {
        var result = await _services.Activity.FindAsync(id);
        Console.WriteLine(result);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     *
     ****/
    [HttpPost]
    public async Task<ActionResult<ActivityDto>> CreateActivity(
        [FromBody] ActivityCreateDto createDto
    )
    {
        var result = await _services.Activity.CreateAsync(createDto);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     *
     ****/
    [HttpPut("{id}")]
    public async Task<ActionResult<ActivityDto>> UpdateActivity(
        int id,
        [FromBody] ActivityUpdateDto updateDto
    )
    {
        var result = await _services.Activity.UpdateAsync(id, updateDto);
        return result != null ? Ok(result) : NotFound();
    }

    /*
     *
     ****/
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActivity(int id, [FromBody] ActivityDto dto)
    {
        var result = await _services.Activity.DeleteAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /* DEPRECATED
     **********************************************************************/

    /*
     *
     ****/
    [HttpPatch("{id}")]
    public async Task<ActionResult<ActivityDto>> PatchActivity(
        [FromRoute] int id,
        [FromBody] JsonPatchDocument<ActivityDto> patchDocument
    )
    {
        var activityToPatch = await _services.Activity.FindAsync(id);
        if (activityToPatch == null)
            return NotFound();

        if (
            !TryValidateAndApplyPatch(
                patchDocument,
                activityToPatch,
                out IActionResult errorResponse
            )
        )
        {
            return BadRequest(errorResponse);
        }

        return Ok(await _services.Activity.PatchActivity(activityToPatch));
    }
}
