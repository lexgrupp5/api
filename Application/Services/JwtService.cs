using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Configuration;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService(IOptions<JwtOptions> jwtOptions) : IJwtService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateToken(User user)
    {
        var credentials = CreateSigningCredentials(_jwtOptions.Secret);
        var claims = CreateClaims(user, new List<string> { "Student", "Teacher" });
        var tokenOptions = CreateTokenOptions(_jwtOptions, credentials, claims);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return accessToken;
    }

    public SecurityToken ValidateToken(string token)
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
    }

    private static JwtSecurityToken CreateTokenOptions(
        JwtOptions options,
        SigningCredentials credentials,
        IEnumerable<Claim> claims
    )
    {
        return new(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
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
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private static SigningCredentials CreateSigningCredentials(string key)
    {
        return new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
    }
}

public interface IJwtService
{
    string GenerateToken(User user);
    SecurityToken ValidateToken(string token);
}



/*
public class IdentityService2 : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(
        string fullName,
        string userName,
        string email,
        string password
    )
    {
        var user = new ApplicationUser
        {
            FullName = fullName,
            UserName = userName,
            Email = email,
        };
        var result = await _userManager.CreateAsync(user, password);
        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> UserExists(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthenticateAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return false;
        }
        var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
        return result.Succeeded;
    }

    public async Task<Result> AddToRolesAsync(string userId, List<string> roles)
    {
        var administratorRole = new IdentityRole(Roles.Administrator);
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
        if (user == null)
        {
            return Result.Failure(new List<string> { "User not found" });
        }
        foreach (var role in roles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        var result = await _userManager.AddToRolesAsync(user, roles);
        return result.ToApplicationResult();
    }
}
 */

/*
public class JwtUtils : IJwtUtils
{
    private readonly IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(
        string userId,
        string fullName,
        string userName,
        IList<string> roles
    )
    {
        var jwtSettings = _configuration.GetSection("JwtOptions");
        Guard.Against.Null(jwtSettings, message: "JwtOptions not found.");
        var key = Guard.Against.NullOrEmpty(
            jwtSettings["Secret"],
            message: "'Secret' not found or empty."
        );
        var issuer = Guard.Against.NullOrEmpty(
            jwtSettings["Issuer"],
            message: "'Issuer' not found or empty."
        );
        var audience = Guard.Against.NullOrEmpty(
            jwtSettings["Audience"],
            message: "'Audience' not found or empty."
        );
        var expiryMinutes = Guard.Against.NullOrEmpty(
            jwtSettings["expiryInMinutes"],
            message: "'expiryInMinutes' not found or empty."
        );
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
    {
        new Claim(JwtRegisteredClaimNames.Sub, userName),
        new Claim(JwtRegisteredClaimNames.Jti, userId),
        new Claim("Name", fullName),
        new Claim("UserId", userId),
    };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(expiryMinutes)),
            signingCredentials: signingCredentials
        );
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return encodedToken;
    }

    public List<string> ValidateToken(string token)
    {
        if (token == null)
            return new List<string>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSettings = _configuration.GetSection("JwtOptions");
        Guard.Against.Null(jwtSettings, message: "JwtOptions not found.");
        var key = Guard.Against.NullOrEmpty(
            jwtSettings["Secret"],
            message: "'Secret' not found or empty."
        );
        tokenHandler.ValidateToken(
            token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            },
            out SecurityToken validatedToken
        );
        var jwtToken = (JwtSecurityToken)validatedToken;
        if (jwtToken != null)
        {
            var roles = new List<string>();
            foreach (var claim in jwtToken.Claims)
            {
                if (claim.Type.ToLower() == "role")
                {
                    roles.Add(claim.Value);
                }
            }
            return roles;
        }
        // return user roles from JWT token if validation successful
        return new List<string>();
    }
}
 */
