using Application.Interfaces;
using Application.Models;
using Domain.Configuration;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class IdentityService(
    IDataCoordinator dataCoordinator,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService,
    TokenConfig tokenConfiguration
) : ServiceBase<User>, IIdentityService
{
    private readonly IDataCoordinator _dc = dataCoordinator;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly ITokenService _tService = tokenService;
    private readonly TokenConfig _tConfig = tokenConfiguration;

    public async Task<(string, RefreshCookieParameter)> AuthenticateAsync(UserAuthModel userDto)
    {
        var user = await ValidateUserAsync(userDto);
        var (access, refresh) = CreateTokens(user);

        var userSession = new UserSession()
        {
            RefreshToken = refresh,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tConfig.Access.ExpirationInMinutes),
            User = user
        };

        var cookieParam = CreateRefreshCookieParameter(refresh);

        _dc.Users.AddUserSession(userSession);
        await _dc.CompleteAsync();

        return new(access, cookieParam);
    }

    public async Task<(string, RefreshCookieParameter)> RefreshTokensAsync(
        string oldAccess,
        string oldRefresh
    )
    {
        var principal =
            _tService.GetPrincipalFromExpiredToken(oldAccess, _tConfig.Access.Secret)
            ?? throw new SecurityTokenException("Invalid token");

        var userSession = await _dc.Users.GetUserSessionAsync(oldRefresh);
        if (userSession == null || userSession.ExpiresAt < DateTime.UtcNow)
        {
            if (userSession != null)
            {
                _dc.Users.RemoveUserSession(userSession);
                await _dc.CompleteAsync();
            }
            Unauthorized("Refresh token is invalid");
        }

        if (principal.Identity?.Name != userSession.User.UserName)
            Unauthorized("User mismatch.");

        var (newAccess, newRefresh) = CreateTokens(userSession.User);

        var newCookieParam = CreateRefreshCookieParameter(newRefresh);

        userSession.RefreshToken = newRefresh;
        await _dc.CompleteAsync();

        return new(newAccess, newCookieParam);
    }

    public async Task<bool> RevokeAsync(string access, string refresh)
    {
        var userSession = await _dc.Users.GetUserSessionAsync(refresh);
        if (userSession == null)
            NotFound();

        _dc.Users.RemoveUserSession(userSession);
        await _dc.CompleteAsync();

        return true;
    }

    public Task<bool> RevokeAllAsync(User user) => throw new NotImplementedException();

    private (string access, string refresh) CreateTokens(User user)
    {
        var accessToken = _tService.GenerateAccessToken(user);
        var refreshToken = _tService.GenerateRefreshToken();

        return (accessToken, refreshToken);
    }

    private async Task<User> ValidateUserAsync(UserAuthModel userDto)
    {
        var user = await _userManager.FindByNameAsync(userDto.UserName);
        if (user == null)
            NotFound();

        var isValidPassword = await _userManager.CheckPasswordAsync(user, userDto.Password);
        if (!isValidPassword)
            Unauthorized("Invalid password");

        return user;
    }

    private RefreshCookieParameter CreateRefreshCookieParameter(string refreshToken) =>
        new(
            refreshToken,
            new()
            {
                HttpOnly = _tConfig.Refresh.HttpOnly,
                SameSite = _tConfig.Refresh.SameSite,
                Secure = _tConfig.Refresh.Secure,
                Expires = DateTime.UtcNow.AddMinutes(_tConfig.Refresh.ExpirationInMinutes),
            }
        );
}
