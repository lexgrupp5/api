using Application.Models;

using Domain.DTOs;

using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<IdentityResult> CreateUserAsync(UserCreateModel newUser);

    Task<string> AuthenticateAsync(UserAuthenticateModel userDto);

    Task<IEnumerable<UserDto>> GetStudentsAsync();
}
