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
        var (access, refresh) = await CreateTokensAsync(user);

        return new(access, refresh);
    }

    public async Task<UserTokenModel> RefreshAsync(UserTokenModel oldTokens)
    {
        var principal =
            _tokenService.GetPrincipalFromExpiredToken(oldTokens.AccessToken, _tokenConfig.Secret)
            ?? throw new SecurityTokenException("Invalid token");

        var userSession = await _dc.Users.GetUserSessionAsync(oldTokens.RefreshToken);
        if (userSession == null)
            Unauthorized("Refresh token is invalid");

        _dc.Users.RemoveUserSession(userSession);

        if (userSession.ExpiresAt < DateTime.UtcNow)
        {
            await _dc.CompleteAsync();
            Unauthorized("Refresh token has expired");
        }

        var (access, refresh) = await CreateTokensAsync(userSession.User);        

        return new(access, refresh);
    }

    public async Task<bool> RevokeAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RevokeByTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RevokeAllAsync() => throw new NotImplementedException();

    private async Task<(string, string)> CreateTokensAsync(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var userSession = new UserSession() {
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenConfig.ExpirationMinutes),
            User = user
        };

        _dc.Users.AddUserSession(userSession);
        await _dc.CompleteAsync();

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
