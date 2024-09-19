using Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<IdentityResult> CreateUserAsync(UserCreateModel newUser);

    Task<string> AuthenticateAsync(UserAuthenticateModel userDto);
}
