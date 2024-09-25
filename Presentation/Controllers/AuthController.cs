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
    public async Task<IActionResult> Login([FromBody] UserAuthModel userDto)
    {
        var tokens = await _services.Identity.AuthenticateAsync(userDto);
        return tokens == null ? BadRequest() : Ok(tokens);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] UserTokenModel tokens)
    {
        await _services.Identity.RevokeAsync(tokens);
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(UserTokenModel oldTokens)
    {
        var newTokens = await _services.Identity.RefreshTokensAsync(oldTokens);
        return newTokens == null ? BadRequest() : Ok(newTokens);
    }
}
