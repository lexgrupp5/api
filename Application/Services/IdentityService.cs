using Application.Interfaces;
using Application.Models;
using Domain.Configuration;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Coordinators;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class IdentityService(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService,
    TokenConfiguration tokenConfiguration,
    IDataCoordinator dataCoordinator
) : ServiceBase<User>, IIdentityService
{
    private readonly IDataCoordinator _dc = dataCoordinator;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly TokenConfiguration _tokenConfig = tokenConfiguration;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<UserTokenModel> AuthenticateAsync(UserAuthModel userDto)
    {
        var user = await ValidateUserAsync(userDto);
        var (access, refresh) = CreateTokens(user);

        var userSession = new UserSession()
        {
            RefreshToken = refresh,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenConfig.RefreshExpirationInMinutes),
            User = user
        };

        _dc.Users.AddUserSession(userSession);
        await _dc.CompleteAsync();

        return new(access, refresh);
    }

    public async Task<UserTokenModel> RefreshTokensAsync(UserTokenModel oldTokens)
    {
        var principal =
            _tokenService.GetPrincipalFromExpiredToken(oldTokens.AccessToken, _tokenConfig.Secret)
            ?? throw new SecurityTokenException("Invalid token");

        var userSession = await _dc.Users.GetUserSessionAsync(oldTokens.RefreshToken);
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
        userSession.RefreshToken = newRefresh;
        await _dc.CompleteAsync();

        return new(newAccess, newRefresh);
    }

    public async Task<bool> RevokeAsync(UserTokenModel tokens)
    {
        var userSession = await _dc.Users.GetUserSessionAsync(tokens.RefreshToken);
        if (userSession == null)
            NotFound();

        _dc.Users.RemoveUserSession(userSession);
        await _dc.CompleteAsync();

        return true;
    }

    public async Task<bool> RevokeAllAsync(User user) => throw new NotImplementedException();

    private (string access, string refresh) CreateTokens(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

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
}
