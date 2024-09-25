using System.Security.Claims;

using Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);

    UserRefreshToken GenerateRefreshToken(User user);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret);
}
