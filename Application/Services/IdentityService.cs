using Application.Interfaces;
using Application.Models;
using Domain.Configuration;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Coordinators;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class IdentityService(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService,
    AuthTokenOptions authTokenOptions,
    IDataCoordinator dataCoordinator
) : ServiceBase<User>, IIdentityService
{
    private readonly IDataCoordinator _db = dataCoordinator;
    private readonly UserManager<User> _user = userManager;
    private readonly RoleManager<IdentityRole> _role = roleManager;
    private readonly AuthTokenOptions _auth = authTokenOptions;
    private readonly ITokenService _token = tokenService;

    public async Task<UserTokenModel> AuthenticateAsync(UserAuthModel userDto)
    {
        var user = await ValidateUserAsync(userDto);
        var (access, refresh) = await CreateTokensAsync(user);

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
        var accessToken = _token.GenerateAccessToken(user);
        var userRefreshToken = _token.GenerateRefreshToken(user);

        _db.Users.AddUserRefreshToken(userRefreshToken);
        await _db.CompleteAsync();

        return (accessToken, userRefreshToken.Token);
    }

    private async Task<User> ValidateUserAsync(UserAuthModel userDto)
    {
        var user = await _user.FindByNameAsync(userDto.UserName);
        if (user == null)
            NotFound();

        var isValidPassword = await _user.CheckPasswordAsync(user, userDto.Password);
        if (!isValidPassword)
            Unauthorized("Invalid password");

        return user;
    }
}
