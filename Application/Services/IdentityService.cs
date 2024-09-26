using System.Security.Claims;
using Application.Interfaces;
using Application.Models;
using Domain.Configuration;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Application.DTOs;

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

    public async Task<TokenResult> AuthenticateAsync(UserAuthModel userDto)
    {
        var user = await ValidateUser(userDto);
        var userRoles = await _userManager.GetRolesAsync(user);
        var access = _tService.GenerateAccessToken(user, userRoles);
        var refresh = _tService.GenerateRefreshToken();

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

    public async Task<TokenResult> RefreshTokensAsync(string oldAccess, string oldRefresh)
    {
        var user = await GetUserFromAccessToken(oldAccess);
        var userRoles = await _userManager.GetRolesAsync(user);
        var userSession = await GetUserSession(oldRefresh, user);
        var newAccess = _tService.GenerateAccessToken(user, userRoles);
        var newRefresh = _tService.GenerateRefreshToken();
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

    private async Task<User> GetUserFromAccessToken(string token)
    {
        var principal = _tService.GetPrincipalFromExpiredToken(token, _tConfig.Access.Secret);
        if (principal == null)
            Unauthorized("Invalid token");

        var user = await FindUserByPrincipalAsync(principal);
        if (user == null)
            NotFound();

        return user;
    }

    private async Task<UserSession> GetUserSession(string refreshToken, User user)
    {
        var userSession = await _dc.Users.GetUserSessionAsync(refreshToken);
        if (userSession == null)
            NotFound();

        if (userSession.ExpiresAt < DateTime.UtcNow)
        {
            _dc.Users.RemoveUserSession(userSession);
            await _dc.CompleteAsync();
            Unauthorized("Refresh token has expired");
        }

        if (user.UserName != userSession.User.UserName)
            Unauthorized("User mismatch.");

        return userSession;
    }

    private async Task<User?> FindUserByPrincipalAsync(ClaimsPrincipal principal) =>
        await _userManager.FindByNameAsync(
            principal.FindFirst(ClaimTypes.NameIdentifier)?.ToString() ?? string.Empty
        );

    private async Task<User> ValidateUser(UserAuthModel userDto)
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
