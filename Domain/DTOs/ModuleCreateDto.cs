namespace Domain.DTOs;

#nullable disable

public record ModuleCreateDto
{
    public string Name { get; set; }
    public int CourseId { get; set; }
    public string Description { get; set; }
}
