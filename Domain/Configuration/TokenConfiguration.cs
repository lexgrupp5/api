using System.ComponentModel.DataAnnotations;

namespace Domain.Configuration;

#nullable disable

public record TokenConfiguration
{
    [Required]
    public string Secret { get; init; }

    [Required]
    public string Issuer { get; init; }

    [Required]
    public string Audience { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "ExpirationMinutes must be greater than 0")]
    public int AccessExpirationInMinutes { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "ExpirationMinutes must be greater than 0")]
    public int RefreshExpirationInMinutes { get; init; }
}