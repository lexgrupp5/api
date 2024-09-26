using System.Security.Claims;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user, ICollection<string>? roles);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret);
}
