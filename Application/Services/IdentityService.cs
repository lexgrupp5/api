using Domain.Configuration;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Models;
using Application.Interfaces;

namespace Application.Services;

public class IdentityService(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IJwtService jwtService,
    JwtOptions jwtOptions
) : ServiceBase<User>, IIdentityService
{
    private readonly UserManager<User> _userManager = userManager;

    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    private readonly JwtOptions _jwtOptions = jwtOptions;

    private readonly IJwtService _jwtService = jwtService;

    public async Task<IdentityResult> CreateUserAsync(UserCreateModel newUser)
    {
        var user = new User
        {
            UserName = newUser.UserName,
            Name = newUser.Name,
            Email = newUser.Email
        };

        return await _userManager.CreateAsync(user, newUser.Password);
    }


    public async Task<string> AuthenticateAsync(UserAuthenticateModel userDto)
    {
        var user = await ValidateUser(userDto) ?? throw new Exception("User not found");
        var token = CreateToken(user);
        return token;
    }

    
    private async Task<User?> ValidateUser(UserAuthenticateModel userDto)
    {
        var user = await _userManager.FindByNameAsync(userDto.UserName);
        if (user == null)
            return null;

        return !await _userManager.CheckPasswordAsync(user, userDto.Password) ? null : user;
    }


    private string CreateToken(User user)
    {
        return _jwtService.GenerateToken(user);
    }
}
