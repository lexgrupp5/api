using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

#nullable disable

public record ModuleCreateDto
{
    [Required]
    public string Name { get; init; }
    
    [Required]
    public string Description { get; init; }

    [Required]
    public int CourseId { get; init; }
}
