namespace Domain.DTOs;

#nullable disable

public record ModuleDto
{
    public int Id { get; init; }
    public int CourseId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public List<ActivityDto> Activities { get; init; }
}
    