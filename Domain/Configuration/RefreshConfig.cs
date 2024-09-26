using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Configuration;

public class RefreshConfig
{
    [Required]
    public int ExpirationInMinutes { get; init; }

    [Required]
    public bool HttpOnly { get; init; }

    [Required]
    public SameSiteMode SameSite { get; init; }

    [Required]
    public bool Secure { get; init; }
}
