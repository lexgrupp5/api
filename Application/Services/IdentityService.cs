using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Configuration;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class IdentityService : ServiceBase<User, UserDto>, IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tService;
    private readonly TokenConfig _tConfig;

    public IdentityService(
        IMapper autoMapper,
        IDataCoordinator dataCoordinator,
        UserManager<User> userManager,
        ITokenService tokenService,
        TokenConfig tokenConfiguration
    )
        : base(dataCoordinator, autoMapper)
    {
        _userManager = userManager;
        _tService = tokenService;
        _tConfig = tokenConfiguration;
    }

    // TODO: refactor
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

        _data.Users.AddUserSession(userSession);
        await _data.CompleteAsync();

        return new(access, cookieParam);
    }

    // TODO: refactor
    public async Task<TokenResult> RefreshTokensAsync(string oldAccess, string oldRefresh)
    {
        var user = await GetUserFromAccessTokenAsync(oldAccess);
        var userRoles = await _userManager.GetRolesAsync(user);
        var userSession = await GetUserSession(oldRefresh, user);
        var newAccess = _tService.GenerateAccessToken(user, userRoles);
        var newRefresh = _tService.GenerateRefreshToken();
        var newCookieParam = CreateRefreshCookieParameter(newRefresh);
        userSession.RefreshToken = newRefresh;
        await _data.CompleteAsync();

        return new(newAccess, newCookieParam);
    }

    // TODO: refactor
    public async Task<bool> RevokeAsync(string access, string refresh)
    {
        var userSession = await _data.Users.GetUserSessionAsync(refresh);
        if (userSession == null)
            NotFound();

        _data.Users.RemoveUserSession(userSession);
        await _data.CompleteAsync();

        return true;
    }

    // TODO: refactor
    public Task<bool> RevokeAllAsync(User user) => throw new NotImplementedException();

    // TODO: refactor
    private async Task<User> GetUserFromAccessTokenAsync(string token)
    {
        var principal = _tService.GetPrincipalFromExpiredToken(token, _tConfig.Access.Secret);
        if (principal == null)
            Unauthorized("Invalid token");

        var user = await FindUserByPrincipalAsync(principal);
        if (user == null)
            NotFound();

        return user;
    }

    // TODO: refactor
    private async Task<UserSession> GetUserSession(string refreshToken, User user)
    {
        var userSession = await _data.Users.GetUserSessionAsync(refreshToken);
        if (userSession == null)
            NotFound();

        if (userSession.ExpiresAt < DateTime.UtcNow)
        {
            _data.Users.RemoveUserSession(userSession);
            await _data.CompleteAsync();
            Unauthorized("Refresh token has expired");
        }

        if (user.UserName != userSession.User.UserName)
            Unauthorized("User mismatch.");

        return userSession;
    }

    // TODO: refactor
    private async Task<User?> FindUserByPrincipalAsync(ClaimsPrincipal principal) =>
        await _userManager.FindByNameAsync(
            principal.FindFirst(ClaimTypes.NameIdentifier)?.ToString() ?? string.Empty
        );

    // TODO: refactor
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

    // TODO: refactor
    private RefreshCookieParameter CreateRefreshCookieParameter(string refreshToken) =>
        new(
            refreshToken,
            new()
            {
                HttpOnly = _tConfig.Refresh.HttpOnly,
                SameSite = _tConfig.Refresh.SameSite,
                Secure = _tConfig.Refresh.Secure,
                Expires = DateTime.UtcNow.AddMinutes(_tConfig.Refresh.ExpirationInMinutes)
            }
        );
}
