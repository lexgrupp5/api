namespace Domain.DTOs;

public record CourseUpdateDto
{
    public int Id { get; init; }
    public string? Name { get; init; } = null;
    public string? Description { get; init; } = null;
    public int? TeacherId { get; init; } = null;
}
