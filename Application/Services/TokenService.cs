using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Domain.Configuration;
using Domain.Constants;
using Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService : ITokenService
{
    /* private readonly AuthTokenOptions _jwtOptions = jwtOptions.Value; */

    private readonly TokenConfiguration _tc;

    public TokenService(TokenConfiguration tokenConfig)
    {
        _tc = tokenConfig;

        _tc = new TokenConfiguration() {
            Secret = "Z7yCwJqQBrpqTEx9UmzXiedyzWSPF6cM",
            Issuer = "myApp.com",
            Audience = "myApp.com",
            AccessExpirationInMinutes = 60
        };

        Console.WriteLine($"After: {_tc.Secret}");
    }

    public string GenerateAccessToken(User user)
    {
        var credentials = CreateSigningCredentials(_tc.Secret);
        var claims = CreateClaims(user, UserRoles.All);
        var tokenOptions = CreateTokenOptions(_tc, credentials, claims);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return accessToken;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        var token = Convert.ToBase64String(randomNumber);
        return token;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken
        );
        var isValid =
            securityToken is JwtSecurityToken jwtSecurityToken
            && jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase
            );

        return isValid ? principal : throw new SecurityTokenException("Invalid token");
    }

    /* public SecurityToken ValidateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = key,
            ValidateLifetime = true,
        };

        handler.ValidateToken(token, validationParameters, out var validatedToken);
        return validatedToken;
    } */



    private static JwtSecurityToken CreateTokenOptions(
        TokenConfiguration options,
        SigningCredentials credentials,
        IEnumerable<Claim> claims
    )
    {
        return new(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(options.AccessExpirationInMinutes),
            signingCredentials: credentials
        );
    }

    private static List<Claim> CreateClaims(User user, IEnumerable<string>? roles)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        if (roles == null)
            return claims;

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }

    private static SigningCredentials CreateSigningCredentials(string key)
    {
        return new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );
    }
}
