using Application.Interfaces;
using Application.Models;

using Domain.Configuration;
using Domain.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

using Presentation.Filters;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ValidateInput]
public class AuthController : ApiBaseController
{
    private readonly IServiceCoordinator _services;
    private readonly string _refreshToken = "RefreshToken";

    public AuthController(IServiceCoordinator services)
    {
        _services = services;
    }

    /*
     *
     ****/
    /* [SkipValidation] */
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserAuthModel userDto)
    {
        /* return BadRequest("test"); */
        var refreshCookiePath = GetControllerPath();
        var (access, cookie) = await _services.Identity.AuthenticateAsync(userDto, refreshCookiePath);

        HttpContext.Response.Cookies.Append(cookie.Key, cookie.Token, cookie.Options);
        
        return access == null ? BadRequest() : Ok(access);
    }

    /*
     *
     ****/
    /* [SkipValidation] */
    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] TokenDto token)
    {
        var refresh = HttpContext.Request.Cookies[_refreshToken];
        if (refresh == null)
            return BadRequest();

        var cookieOptions = _services.Identity.RefreshCookieBaseOptions(GetControllerPath());
        HttpContext.Response.Cookies.Delete(_refreshToken, cookieOptions);

        await _services.Identity.RevokeAsync(token.AccessToken, refresh);
        return NoContent();
    }

    /*
     *
     ****/
    /* [SkipValidation] */
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<string>> RefreshToken([FromBody] TokenDto token)
    {
            var refresh = HttpContext.Request.Cookies[_refreshToken];
        if (refresh == null)
            return BadRequest();

        var (newAccess, cookie) = await _services.Identity.RefreshTokensAsync(
            token.AccessToken, refresh, GetControllerPath());

        cookie.Options.Path = GetControllerPath();
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
