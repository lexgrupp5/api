using Application.Interfaces;
using Application.Models;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers;

[ApiController]
[Route("api/activities")]
[ValidateInput]
[Produces("application/json")]
public class ActivityController : ControllerBase
{
    private readonly IServiceCoordinator _services;

    public ActivityController(IServiceCoordinator services)
    {
        _services = services;
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<ActivityDto>>> GetActivities()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public Task<ActionResult<ActivityDto>> GetActivityById(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task<ActionResult<ActivityDto>> CreateActivity([FromBody] ActivityCreateModel activity)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public Task<ActionResult<ActivityDto>> UpdateActivity(int id, [FromBody] ActivityDto activity)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> DeleteActivity(int id)
    {
        throw new NotImplementedException();
    }
}
