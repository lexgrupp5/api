using Application.Models;
using Domain.DTOs;
using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<UserTokenModel> AuthenticateAsync(UserAuthModel userDto);
    
    Task<UserTokenModel> RefreshTokensAsync(UserTokenModel oldTokens);

    Task<bool> RevokeAsync(UserTokenModel tokens);

    Task<bool> RevokeAllAsync(User user);
}
