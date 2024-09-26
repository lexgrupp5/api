using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthController(IServiceCoordinator serviceCoordinator) : ControllerBase
{
    private readonly IServiceCoordinator _services = serviceCoordinator;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] UserAuthModel userDto)
    {
        var (access, cookie) = await _services.Identity.AuthenticateAsync(userDto);
        
        HttpContext.Response.Cookies.Append(cookie.Key, cookie.Token, cookie.Options);
        
        return access == null ? BadRequest() : Ok(access);
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout([FromHeader] string access)
    {
        var refresh = HttpContext.Request.Cookies["RefreshToken"];
        if (refresh == null)
            return BadRequest();

        HttpContext.Response.Cookies.Delete("RefreshToken");

        await _services.Identity.RevokeAsync(access, refresh);
        return Ok();
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> RefreshToken([FromHeader] string access)
    {
        var refresh = HttpContext.Request.Cookies["RefreshToken"];
        if (refresh == null)
            return BadRequest();

        var (newAccess, cookie) = await _services.Identity.RefreshTokensAsync(access, refresh);

        HttpContext.Response.Cookies.Append(cookie.Key, cookie.Token, cookie.Options);

        return newAccess == null ? BadRequest() : Ok(newAccess);
    }
}
