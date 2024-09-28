using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ValidateInput]
public class AuthController(IServiceCoordinator serviceCoordinator) : ControllerBase
{
    private readonly IServiceCoordinator _services = serviceCoordinator;

    /*
     *
     ****/
    /* [SkipValidation] */
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserAuthModel userDto)
    {
        /* return BadRequest("test"); */
        var (access, cookie) = await _services.Identity.AuthenticateAsync(userDto);
        
        HttpContext.Response.Cookies.Append(cookie.Key, cookie.Token, cookie.Options);
        
        return access == null ? BadRequest() : Ok(access);
    }

    /*
     *
     ****/
    /* [SkipValidation] */
    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromHeader] string access)
    {
        var refresh = HttpContext.Request.Cookies["RefreshToken"];
        if (refresh == null)
            return BadRequest();

        HttpContext.Response.Cookies.Delete("RefreshToken");

        await _services.Identity.RevokeAsync(access, refresh);
        return NoContent();
    }

    /*
     *
     ****/
    /* [SkipValidation] */
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<string>> RefreshToken([FromHeader] string access)
    {
        var refresh = HttpContext.Request.Cookies["RefreshToken"];
        if (refresh == null)
            return BadRequest();

        var (newAccess, cookie) = await _services.Identity.RefreshTokensAsync(access, refresh);

        HttpContext.Response.Cookies.Append(cookie.Key, cookie.Token, cookie.Options);

        return newAccess == null ? BadRequest() : Ok(newAccess);
    }

    /*
     *
     ****/
    /* [SkipValidation] */
    [AllowAnonymous]
    [HttpGet("roles")]
    public Task<ActionResult> ListUserRoles()
    {
        throw new NotImplementedException();
    }
}
