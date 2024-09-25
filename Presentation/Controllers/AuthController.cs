using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IServiceCoordinator serviceCoordinator) : ControllerBase
{
    private readonly IServiceCoordinator _sc = serviceCoordinator;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserAuthModel userDto)
    {
        var token = await _sc.Identity.AuthenticateAsync(userDto);
        return Ok(token);        
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }

}

