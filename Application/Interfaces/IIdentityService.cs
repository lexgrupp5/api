using Application.Models;
using Application.Services;

using Domain.Entities;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<(string, RefreshCookieParameter)> AuthenticateAsync(UserAuthModel userDto);
    
    Task<(string, RefreshCookieParameter)> RefreshTokensAsync(string accessToken, string refreshToken);

    Task<bool> RevokeAsync(string accessToken, string refreshToken);

    Task<bool> RevokeAllAsync(User user);

    
}
