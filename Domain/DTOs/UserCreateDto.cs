using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

#nullable disable

public record UserCreateDto
{
    [Required]
    public string Name { get; init; }

    [Required]
    public string Username { get; init; }

    [Required]
    public string Email { get; init; }

    [Required]
    public string Password { get; init; }
}
