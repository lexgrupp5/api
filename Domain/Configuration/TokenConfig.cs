using System.ComponentModel.DataAnnotations;

namespace Domain.Configuration;

#nullable disable

public class TokenConfig
{
    [Required]
    public AccessConfig Access { get; init; }

    [Required]
    public RefreshConfig Refresh { get; init; }
}
