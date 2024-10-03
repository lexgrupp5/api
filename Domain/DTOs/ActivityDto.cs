namespace Domain.DTOs;

#nullable disable

public record ActivityDto
{
    public int Id { get; init; }
    public string Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    // Module
    public int ModuleId { get; init; }

    // ActivityType
    public string ActivityTypeName { get; init; }
    public string ActivityTypeDescription { get; init; }
}
