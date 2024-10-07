using Application.Models;
using Application.DTOs;
using Application.Services;

using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<TokenResult> AuthenticateAsync(
        UserAuthModel userDto, string? refreshCookiePath);
    
    Task<TokenResult> RefreshTokensAsync(
        string accessToken, string refreshToken, string? refreshCookiePath);

    Task<bool> RevokeAsync(string accessToken, string refreshToken);

    Task<bool> RevokeAllAsync(User user);

    CookieOptions RefreshCookieBaseOptions(string? refreshCookiePath);
}
