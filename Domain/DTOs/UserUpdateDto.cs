using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public record UserUpdateDto
{
    [Required]
    public required string Id { get; init; }

    public string? Name { get; init; } = null;
    public string? Email { get; init; } = null;
    public string? Username { get; init; } = null;
    public string? Password { get; init; } = null;

    // Course
    public int? CourseId { get; init; } = null;
}
