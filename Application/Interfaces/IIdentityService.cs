using Application.Models;
using Application.DTOs;
using Application.Services;

using Domain.Entities;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<TokenResult> AuthenticateAsync(UserAuthModel userDto);
    
    Task<TokenResult> RefreshTokensAsync(string accessToken, string refreshToken);

    Task<bool> RevokeAsync(string accessToken, string refreshToken);

    Task<bool> RevokeAllAsync(User user);

    
}
