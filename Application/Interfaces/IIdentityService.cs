using Application.Models;
using Domain.DTOs;
using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<UserTokenModel> AuthenticateAsync(UserAuthModel userDto);

    Task<bool> RevokeAsync(User user);

    Task<bool> RevokeByTokenAsync(string token);
}
